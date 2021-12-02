using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Text;

namespace Entity.Models
{
    public class ProductMetaData
    {
        [MaxLength(512, ErrorMessage = "Lütfen daha kısa bir isim giriniz.")]
        [Required(ErrorMessage = "Lütfen ürün ismi ekleyiniz.")]
        public string Name { get; set; }
        [MaxLength(64, ErrorMessage = "Lütfen biraz daha kısaltın.")]
        [Required(ErrorMessage = "Lütfen marka ekleyiniz.")]
        public string Brand { get; set; }
        [Required(ErrorMessage = "Lütfen stok adeti ekleyiniz.")]
        [MaxLength(64, ErrorMessage = "Lütfen biraz daha kısaltın.")]
        public string Stock { get; set; }
        [Required(ErrorMessage = "Lütfen alış fiyatı ekleyiniz.")]
        public decimal? PurchasePrice { get; set; }
        [Required(ErrorMessage = "Lütfen satış fiyatı ekleyiniz.")]
        public decimal? SalePrice { get; set; }
        public bool Status { get; set; }
        [Required(ErrorMessage = "Lütfen ürün kategorisi seçiniz.")]
        public int CategoryId { get; set; }
        public int? SaleProcessId { get; set; }
    }
    public class ProductCargoMetaData
    {
        [MaxLength(64, ErrorMessage = "Lütfen daha kısa bir isim giriniz.")]
        [Required(ErrorMessage = "Lütfen isim ekleyiniz.")]
        public string Title { get; set; }
        [Required(ErrorMessage = "Lütfen açıklama ekleyiniz.")]
        public string Description { get; set; }
    }
    public class CargoCompanyMetaData
    {
        public int Id { get; set; }
        [MaxLength(64, ErrorMessage = "Lütfen daha kısa bir isim giriniz.")]
        [Required(ErrorMessage = "Lütfen isim ekleyiniz.")]
        public string Title { get; set; }
        public string Image { get; set; }
        [Required(ErrorMessage = "Lütfen websitesi ekleyiniz.")]
        public string WebSite { get; set; }
    }
    public class CategoryMetaData
    {
        [Required(ErrorMessage = "Lütfen kategori giriniz.")]
        public string Name { get; set; }
    }
    public class DepartmentMetaData
    {
        [Required(ErrorMessage = "Lütfen departman ismi giriniz.")]
        public string Name { get; set; }
    }
    public class StaffMetaData
    {
        [MaxLength(64, ErrorMessage = "Lütfen daha kısa bir isim giriniz.")]
        [Required(ErrorMessage = "Lütfen isminizi giriniz.")]
        public string Name { get; set; }
        [MaxLength(64, ErrorMessage = "Lütfen daha kısa bir soyisim giriniz.")]
        [Required(ErrorMessage = "Lütfen soyisminizi giriniz.")]
        public string Surname { get; set; }
        [MinLength(5, ErrorMessage = "Lütfen daha uzun bir e-posta adresini giriniz.")]
        [MaxLength(64, ErrorMessage = "Lütfen daha kısa bir e-posta adresini giriniz.")]
        [Required(ErrorMessage = "Lütfen e-posta adresi ekleyiniz.")]
        [EmailAddress(ErrorMessage = "Lütfen bir e-posta adresi ekleyiniz.")]

        public string Email { get; set; }
        [MaxLength(64, ErrorMessage = "Lütfen daha kısa bir kullanıcı adı giriniz.")]
        [Required(ErrorMessage = "Lütfen kullanıcı adı giriniz.")]
        public string UserName { get; set; }
        [MinLength(3, ErrorMessage = "Lütfen daha uzun bir şifre giriniz.")]
        [MaxLength(64, ErrorMessage = "Lütfen daha kısa bir şifre giriniz.")]
        [Required(ErrorMessage = "Lütfen şifre ekleyiniz.")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Lütfen departman seçiniz.")]
        public int? DepartmentId { get; set; }
        [Required(ErrorMessage = "Lütfen işe giriş tarihi seçiniz.")]
        public DateTime StartDate { get; set; }
        [Required(ErrorMessage = "Lütfen 6 karakterli güvenlik kodu belirleyiniz.")]
        [Range(99999, 1000000, ErrorMessage = "Lütfen 6 karakterli güvenlik kodu belirleyiniz.")]
        public int? PinCode { get; set; }
    }
    public class EmailMetaData
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Lütfen email adresi ekleyiniz.")]
        public string ReceiverEmail { get; set; }
        [Required(ErrorMessage = "Lütfen açıklama ekleyiniz.")]
        public string Description { get; set; }
        [Required(ErrorMessage = "Lütfen konu belirtiniz.")]
        public string Subject { get; set; }
    }
    public class ToDoListGroupMetaData
    {
        [Required(ErrorMessage = "Lütfen başlık ekleyiniz.")]
        public string Title { get; set; }
        [MaxLength(256, ErrorMessage = "Lütfen açıklamayı kısaltın.")]
        [Required(ErrorMessage = "Lütfen kısa açıklama ekleyiniz.")]
        public string ShortDescription { get; set; }
    }
    public class ContactMetaData
    {
        [MaxLength(128, ErrorMessage = "Lütfen daha kısa bir isim-soyisim giriniz.")]
        [Required(ErrorMessage = "Lütfen isminizi giriniz.")]
        public string Name { get; set; }
        [MaxLength(128, ErrorMessage = "Lütfen daha kısa bir isim-soyisim giriniz.")]
        [Required(ErrorMessage = "Lütfen konu belirtiniz.")]
        public string Subject { get; set; }
        [MinLength(5, ErrorMessage = "Lütfen daha uzun bir e-posta adresini giriniz.")]
        [MaxLength(128, ErrorMessage = "Lütfen daha kısa bir e-posta adresini giriniz.")]
        [Required(ErrorMessage = "Lütfen e-posta adresi ekleyiniz.")]
        [EmailAddress(ErrorMessage = "Lütfen bir e-posta adresi ekleyiniz.")]
        public string Email { get; set; }
        [MaxLength(2048, ErrorMessage = "Lütfen daha kısa bir mesaj giriniz. Veya e-posta adresimden bana ulaşın.")]
        [Required(ErrorMessage = "Lütfen mesajınızı giriniz.")]
        public string Message { get; set; }
    }
    public class CurrentMetaData
    {
        [MaxLength(64, ErrorMessage = "Lütfen daha kısa bir kullanıcı adı giriniz.")]
        [Required(ErrorMessage = "Lütfen kullanıcı adı giriniz.")]
        public string UserName { get; set; }
        [MaxLength(64,ErrorMessage = "Lütfen daha kısa bir isim giriniz.")]
        [Required(ErrorMessage = "Lütfen isminizi giriniz.")]
        public string Name { get; set; }
        [MaxLength(64,ErrorMessage = "Lütfen daha kısa bir soyisim giriniz.")]
        [Required(ErrorMessage = "Lütfen soyisminizi giriniz.")]
        public string Surname { get; set; }
        [MinLength(5,ErrorMessage = "Lütfen daha uzun bir e-posta adresini giriniz.")]
        [MaxLength(64,ErrorMessage = "Lütfen daha kısa bir e-posta adresini giriniz.")]
        [Required(ErrorMessage = "Lütfen e-posta adresi ekleyiniz.")]
        [EmailAddress(ErrorMessage = "Lütfen bir e-posta adresi ekleyiniz.")]

        public string Mail { get; set; }
        [MinLength(3, ErrorMessage = "Lütfen daha uzun bir şifre giriniz.")]
        [MaxLength(64, ErrorMessage = "Lütfen daha kısa bir şifre giriniz.")]
        [Required(ErrorMessage = "Lütfen şifre ekleyiniz.")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Lütfen 6 karakterli güvenlik kodu belirleyiniz.")]
        [Range(99999, 1000000, ErrorMessage = "Lütfen 6 karakterli güvenlik kodu belirleyiniz.")]
        public int? PinCode { get; set; }
    }
    public class AdminMetaData
    {
        [MaxLength(64)]
        [Required(ErrorMessage = "Lütfen kullanıcı adı giriniz.")]
        public string UserName { get; set; }
        [MaxLength(64)]
        [Required(ErrorMessage = "Lütfen isim giriniz.")]
        public string Name { get; set; }
        [MaxLength(64)]
        [Required(ErrorMessage = "Lütfen soyisim giriniz.")]
        public string Surname { get; set; }
        [MaxLength(60)]
        [Required(ErrorMessage = "Lütfen mail adresinizi giriniz.")]
        [EmailAddress(ErrorMessage = "Lütfen bir e-posta adresi ekleyiniz.")]
        public string Email { get; set; }
        [MaxLength(60)]
        [Required(ErrorMessage = "Lütfen şifrenizi giriniz.")]
        public string Password { get; set; }
        [MaxLength(300, ErrorMessage = "Lütfen daha kısa bir adres giriniz.")]
        public string Adress { get; set; }
        [Required(ErrorMessage = "Lütfen 6 karakterli güvenlik kodu belirleyiniz.")]
        [Range(99999, 1000000, ErrorMessage = "Lütfen 6 karakterli güvenlik kodu belirleyiniz.")]
        public int? PinCode { get; set; }
    }
    public class RememberPasswordMetaData
    {
        [Required(ErrorMessage = "Lütfen email adresinizi giriniz.")]
        [EmailAddress(ErrorMessage = "Lütfen bir e-posta adresi ekleyiniz.")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Lütfen 6 karakterli güvenlik kodunuzu giriniz.")]
        [Range(99999, 1000000, ErrorMessage = "Lütfen 6 karakter giriniz.")]
        public int? PinCode { get; set; }

    }
    public class NewsletterMetaData
    {
        [EmailAddress(ErrorMessage = "Lütfen bir e-posta adresi ekleyiniz.")]
        [Required(ErrorMessage = "Lütfen email adresinizi giriniz.")]
        public string Email { get; set; }
    }
}
