using admin.ViewModel;
using admin.ViewModels;
using AutoMapper;
using Entity.Context;
using Entity.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Service.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace admin.Hubs
{
    [Authorize]
    public class ChatHub : Hub
    {
        public readonly static List<UserViewModel> _Connections = new List<UserViewModel>();
        public readonly static List<RoomViewModel> _Rooms = new List<RoomViewModel>();
        private readonly static Dictionary<string, string> _ConnectionsMap = new Dictionary<string, string>();
        private readonly IMapper _mapper;

        private readonly SwappDbContext _context;
        private readonly IUnitOfWork _uow;

        public ChatHub(SwappDbContext context, IUnitOfWork uow, IMapper mapper)
        {
            _context = context;
            _uow = uow;
            _mapper = mapper;
        }

        public async Task SendPrivate(int receiverId, string receiverRole, string message)
        {
            int userId = Convert.ToInt32(_uow.Admin.GetIdByAdmin);
            string userRole = _uow.Admin.GetRoleByAdmin;

            //For Only Staff;
            var sender = _Connections.Where(u => u.Id == userId && u.Role == userRole).First();

            if (!string.IsNullOrEmpty(message.Trim()))
            {
                // Build the message
                var messageViewModel = new MessageViewModel()
                {
                    Content = Regex.Replace(message, @"(?i)<(?!img|a|/a|/img).*?>", string.Empty),
                    From = sender.FullName,
                    Avatar = sender.Avatar,
                    AvatarSrc = sender.Role == "user" ? "admin" : sender.Role,
                    Timestamp = DateTime.Now.ToLongTimeString()
                };

                var msg = new Message()
                {
                    SendDate = DateTime.Now,
                    Message1 = Regex.Replace(message, @"(?i)<(?!img|a|/a|/img).*?>", string.Empty),
                    SenderRole = userRole,
                    ReceiverId = receiverId,
                    ReceiverRole = receiverRole,
                    SenderId = userId
                };
                _uow.Message.Add(msg);

                if (receiverRole == "admin" || receiverRole == "user")
                {
                    var user = _uow.Admin.GetById(receiverId);
                    messageViewModel.To = user.Name + " " + user.Surname;
                }
                if (receiverRole == "staff")
                {
                    var user = _uow.Staff.GetById(receiverId);
                    messageViewModel.To = user.Name + " " + user.Surname;
                }
                if (receiverRole == "current")
                {
                    var user = _uow.Current.GetById(receiverId);
                    messageViewModel.To = user.Name + " " + user.Surname;
                }

                // Send the message
                await Clients.Client(userId + "-" + userRole).SendAsync("newMessage", messageViewModel);
                await Clients.Caller.SendAsync("newMessage", messageViewModel);
            }
        }

        public async Task SendToRoom(string roomName, string message)
        {
            try
            {
                int userId = Convert.ToInt32(_uow.Admin.GetIdByAdmin);
                string userRole = _uow.Admin.GetRoleByAdmin;
                var room = _context.MessageRooms.Where(r => r.Name == roomName).FirstOrDefault();

                if (!string.IsNullOrEmpty(message.Trim()))
                {

                    // Create and save message in database
                    var msg = new Message()
                    {
                        SendDate = DateTime.Now,
                        Message1 = Regex.Replace(message, @"(?i)<(?!img|a|/a|/img).*?>", string.Empty),
                        SenderRole = userRole,
                        RoomId = room.Id,
                        SenderId = userId
                    };
                    _uow.Message.Add(msg);

                    ViewMessageAll vmsg = new ViewMessageAll();
                    //userRole control
                    if (userRole == "admin" || userRole == "user")
                    {
                        Admin sender = _uow.Admin.GetById(Convert.ToInt32(userId));
                        vmsg = new ViewMessageAll()
                        {
                            SendDate = DateTime.Now,
                            Message = Regex.Replace(message, @"(?i)<(?!img|a|/a|/img).*?>", string.Empty),
                            SenderRole = "admin",
                            Surname = _uow.Admin.GetById(msg.SenderId).Surname,
                            Name = _uow.Admin.GetById(msg.SenderId).Name,
                            RoomId = room.Id,
                            RoomName = _uow.MessageRoom.GetById(room.Id).Name,
                            Image = _uow.Admin.GetById(msg.SenderId).Image == null ? "default.jpg" : _uow.Admin.GetById(msg.SenderId).Image,
                            UserName = _uow.Admin.GetById(msg.SenderId).UserName,
                        };
                    }
                    else if (userRole == "staff")
                    {
                        Staff sender = _uow.Staff.GetById(Convert.ToInt32(userId));
                        vmsg = new ViewMessageAll()
                        {
                            SendDate = DateTime.Now,
                            Message = Regex.Replace(message, @"(?i)<(?!img|a|/a|/img).*?>", string.Empty),
                            SenderRole = userRole,
                            Surname = _uow.Staff.GetById(msg.SenderId).Surname,
                            Name = _uow.Staff.GetById(msg.SenderId).Name,
                            RoomId = room.Id,
                            RoomName = _uow.MessageRoom.GetById(room.Id).Name,
                            Image = _uow.Staff.GetById(msg.SenderId).Image == null ? "default.jpg" : _uow.Staff.GetById(msg.SenderId).Image,
                            UserName = _uow.Staff.GetById(msg.SenderId).UserName,
                        };
                    }
                    else
                    {
                        Current sender = _uow.Current.GetById(Convert.ToInt32(userId));
                        vmsg = new ViewMessageAll()
                        {
                            SendDate = DateTime.Now,
                            Message = Regex.Replace(message, @"(?i)<(?!img|a|/a|/img).*?>", string.Empty),
                            SenderRole = userRole,
                            Surname = _uow.Current.GetById(msg.SenderId).Surname,
                            Name = _uow.Current.GetById(msg.SenderId).Name,
                            RoomId = room.Id,
                            RoomName = _uow.MessageRoom.GetById(room.Id).Name,
                            Image = _uow.Current.GetById(msg.SenderId).Image == null ? "default.jpg" : _uow.Current.GetById(msg.SenderId).Image,
                            UserName = _uow.Current.GetById(msg.SenderId).UserName,
                        };
                    }

                    // Broadcast the message               
                    var messageViewModel = _mapper.Map<ViewMessageAll, MessageViewModel>(vmsg);
                    messageViewModel.To = "";
                    if (msg.ReceiverRole != null)
                    {
                        messageViewModel.To = "add receiver";
                    }
                    await Clients.Group(roomName).SendAsync("newMessage", messageViewModel);
                }
            }
            catch (Exception)
            {
                await Clients.Caller.SendAsync("onError", "Message not send! Message should be 1-500 characters.");
            }
        }

        public async Task Join(string roomName)
        {
            try
            {

                int userId = Convert.ToInt32(_uow.Admin.GetIdByAdmin);
                string userRole = _uow.Admin.GetRoleByAdmin;
                var room = _context.MessageRooms.Where(r => r.Name == roomName).FirstOrDefault();

                var user = _Connections.Where(u => u.Id == userId && u.Role == userRole).FirstOrDefault();

                if (user != null && user.CurrentRoom != roomName)
                {
                    // Remove user from others list
                    if (!string.IsNullOrEmpty(user.CurrentRoom))
                        await Clients.OthersInGroup(user.CurrentRoom).SendAsync("removeUser", user);

                    // Join to new chat room
                    await Leave(user.CurrentRoom);
                    await Groups.AddToGroupAsync(Context.ConnectionId, roomName);
                    user.CurrentRoom = roomName;

                    // Tell others to update their list of users
                    await Clients.OthersInGroup(roomName).SendAsync("addUser", user);
                }
            }
            catch (Exception ex)
            {
                await Clients.Caller.SendAsync("onError", "You failed to join the chat room!" + ex.Message);
            }
        }

        public async Task Leave(string roomName)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, roomName);
        }

        public async Task CreateRoom(string roomName)
        {
            try
            {

                // Accept: Letters, numbers and one space between words.
                Match match = Regex.Match(roomName, @"^\w+( \w+)*$");
                if (!match.Success)
                {
                    await Clients.Caller.SendAsync("onError", "Invalid room name!\nRoom name must contain only letters and numbers.");
                }
                else if (roomName.Length < 5 || roomName.Length > 100)
                {
                    await Clients.Caller.SendAsync("onError", "Room name must be between 5-100 characters!");
                }
                else if (_context.MessageRooms.Any(r => r.Name == roomName))
                {
                    await Clients.Caller.SendAsync("onError", "Another chat room with this name exists");
                }
                else
                {
                    // Create and save chat room in database
                    int currentId = Convert.ToInt32(_uow.Admin.GetIdByAdmin);
                    string currentRole = _uow.Admin.GetRoleByAdmin;

                    Staff user = _uow.Staff.GetById(Convert.ToInt32(currentId));
                    var room = new MessageRooms()
                    {
                        Name = roomName,
                        AdminRole = currentRole,
                        AdminId = currentId
                    };
                    _uow.MessageRoom.Add(room);

                    if (room != null)
                    {
                        // Update room list
                        var roomViewModel = _mapper.Map<MessageRooms, RoomViewModel>(room);
                        _Rooms.Add(roomViewModel);
                        await Clients.All.SendAsync("addChatRoom", roomViewModel);
                    }
                }
            }
            catch (Exception ex)
            {
                await Clients.Caller.SendAsync("onError", "Couldn't create chat room: " + ex.Message);
            }
        }

        public async Task DeleteRoom(string roomName)
        {
            try
            {
                // Delete from database
                var room = _uow.MessageRoom.GetAll().FirstOrDefault(p => p.Name == roomName);
                _uow.MessageRoom.Delete(room);

                // Delete from list
                var roomViewModel = _Rooms.First(r => r.Name == roomName);
                _Rooms.Remove(roomViewModel);

                // Move users back to Lobby
                await Clients.Group(roomName).SendAsync("onRoomDeleted", string.Format("Room {0} has been deleted.\nYou are now moved to the Lobby!", roomName));

                // Tell all users to update their room list
                await Clients.All.SendAsync("removeChatRoom", roomViewModel);
            }
            catch (Exception)
            {
                await Clients.Caller.SendAsync("onError", "Can't delete this chat room. Only owner can delete this room.");
            }
        }

        public IEnumerable<RoomViewModel> GetRooms()
        {
            // First run?
            if (_Rooms.Count == 0)
            {
                foreach (var room in _uow.MessageRoom.GetAll())
                {
                    var roomViewModel = _mapper.Map<MessageRooms, RoomViewModel>(room);
                    _Rooms.Add(roomViewModel);
                }
            }

            return _Rooms.ToList();
        }

        public IEnumerable<UserViewModel> GetUsers(string roomName)
        {
            return _Connections.Where(u => u.CurrentRoom == roomName).ToList();
        }

        public IEnumerable<MessageViewModel> GetMessageHistory(string roomName)
        {
            var messageHistory = _context.ViewMessageAll.Where(m => m.RoomId == _uow.MessageRoom.GetAll().FirstOrDefault(p => p.Name == roomName).Id)
                    .OrderByDescending(m => m.SendDate)
                    .Take(20)
                    .AsEnumerable()
                    .Reverse()
                    .ToList();
            if (messageHistory.Any(p => p.Image == null))
            {
                List<ViewMessageAll> newList = new List<ViewMessageAll>();
                foreach (var item in messageHistory)
                {
                    if (item.Image == null)
                    {
                        item.Image = "default.jpg";
                    }
                    newList.Add(item);
                }
                return _mapper.Map<IEnumerable<ViewMessageAll>, IEnumerable<MessageViewModel>>(newList);
            }
            return _mapper.Map<IEnumerable<ViewMessageAll>, IEnumerable<MessageViewModel>>(messageHistory);
        }

        public override Task OnConnectedAsync()
        {
            try
            {
                int userId = Convert.ToInt32(_uow.Admin.GetIdByAdmin);
                string userRole = _uow.Admin.GetRoleByAdmin;
                UserViewModel userViewModel = new UserViewModel();
                if (userRole == "admin" || userRole == "user")
                {
                    var user = _uow.Admin.GetById(Convert.ToInt32(userId));
                    userViewModel.Id = userId;
                    userViewModel.Username = user.UserName;
                    userViewModel.Role = userRole;
                    userViewModel.FullName = user.Name + " " + user.Surname;
                    userViewModel.Device = GetDevice();
                    userViewModel.CurrentRoom = "";
                    userViewModel.AvatarSrc = "admin";
                    userViewModel.Avatar = user.Image == null ? "default.jpg" : user.Image;

                    if (!_Connections.Any(u => u.Id == userId && u.Role == userRole))
                    {
                        _Connections.Add(userViewModel);
                        _ConnectionsMap.Add(user.Id + "-" + userRole, Context.ConnectionId);
                    }

                    Clients.Caller.SendAsync("getProfileInfo", user.Name + user.Surname, user.Image, userRole == "user" ? "admin" : userRole);
                }
                if (userRole == "staff")
                {
                    var user = _uow.Staff.GetById(Convert.ToInt32(userId));
                    userViewModel.Id = userId;
                    userViewModel.Username = user.UserName;
                    userViewModel.Role = userRole;
                    userViewModel.FullName = user.Name + " " + user.Surname;
                    userViewModel.Device = GetDevice();
                    userViewModel.CurrentRoom = "";
                    userViewModel.Avatar = user.Image == null ? "default.jpg" : user.Image;
                    userViewModel.AvatarSrc = "staff";

                    if (!_Connections.Any(u => u.Id == userId && u.Role == userRole))
                    {
                        _Connections.Add(userViewModel);
                        _ConnectionsMap.Add(user.Id + "-" + userRole, Context.ConnectionId);
                    }

                    Clients.Caller.SendAsync("getProfileInfo", user.Name + user.Surname, user.Image, userRole == "user" ? "admin" : userRole);
                }
                if (userRole == "current")
                {
                    var user = _uow.Current.GetById(Convert.ToInt32(userId));
                    userViewModel.Id = userId;
                    userViewModel.Username = user.UserName;
                    userViewModel.Role = userRole;
                    userViewModel.FullName = user.Name + " " + user.Surname;
                    userViewModel.Device = GetDevice();
                    userViewModel.CurrentRoom = "";
                    userViewModel.AvatarSrc = "current";
                    userViewModel.Avatar = user.Image == null ? "default.jpg" : user.Image;
                    if (!_Connections.Any(u => u.Id == userId && u.Role == userRole))
                    {
                        _Connections.Add(userViewModel);
                        _ConnectionsMap.Add(user.Id + "-" + userRole, Context.ConnectionId);
                    }

                    Clients.Caller.SendAsync("getProfileInfo", user.Name + user.Surname, user.Image, userRole == "user" ? "admin" : userRole);
                }
            }
            catch (Exception ex)
            {
                Clients.Caller.SendAsync("onError", "OnConnected:" + ex.Message);
            }
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            try
            {
                int userId = Convert.ToInt32(_uow.Admin.GetIdByAdmin);
                string userRole = _uow.Admin.GetRoleByAdmin;
                var user = _Connections.Where(u => u.Id == userId && u.Role == userRole).First();
                _Connections.Remove(user);

                // Tell other users to remove you from their list
                Clients.OthersInGroup(user.CurrentRoom).SendAsync("removeUser", user);

                // Remove mapping
                _ConnectionsMap.Remove(userId + "-" + userRole);
            }
            catch (Exception ex)
            {
                Clients.Caller.SendAsync("onError", "OnDisconnected: " + ex.Message);
            }

            return base.OnDisconnectedAsync(exception);
        }

        private string GetDevice()
        {
            var device = Context.GetHttpContext().Request.Headers["Device"].ToString();
            if (!string.IsNullOrEmpty(device) && (device.Equals("Desktop") || device.Equals("Mobile")))
                return device;

            return "Web";
        }
    }
}
