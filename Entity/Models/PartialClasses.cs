using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Entity.Models
{
    [ModelMetadataType(typeof(ProductMetaData))]
    public partial class Product
    {
    }
    [ModelMetadataType(typeof(DepartmentMetaData))]
    public partial class Department
    {
    }
    [ModelMetadataType(typeof(CategoryMetaData))]
    public partial class Category
    {
    }
    [ModelMetadataType(typeof(AdminMetaData))]
    public partial class Admin
    {
    }
    [ModelMetadataType(typeof(RememberPasswordMetaData))]
    public partial class RememberPassword
    {
    }
    [ModelMetadataType(typeof(CargoCompanyMetaData))]
    public partial class CargoCompany
    {
    }
    [ModelMetadataType(typeof(StaffMetaData))]
    public partial class Staff
    {
    }
    [ModelMetadataType(typeof(CurrentMetaData))]
    public partial class Current
    {
    }
    [ModelMetadataType(typeof(EmailMetaData))]
    public partial class Email
    {
    }
    [ModelMetadataType(typeof(ToDoListGroupMetaData))]
    public partial class ToDoListGroup
    {
    }
    [ModelMetadataType(typeof(ContactMetaData))]
    public partial class Contact
    {
    }
    [ModelMetadataType(typeof(NewsletterMetaData))]
    public partial class Newsletter
    {
    }
    [ModelMetadataType(typeof(ProductCargoMetaData))]
    public partial class ProductCargo
    {
    }
}
