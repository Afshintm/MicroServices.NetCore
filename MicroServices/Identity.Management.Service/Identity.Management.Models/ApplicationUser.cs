using Microsoft.AspNetCore.Identity;
using System;

namespace Identity.Management.Models
{
    public class ApplicationUser : IdentityUser
    {
//        private UserReference _userRef;
        public ApplicationUser()
        {
            Id = Guid.NewGuid().ToString();
  //          _userRef = new UserReference() { Id = Id };
        }

        public ApplicationUser(string userName) : base(userName) { }

        //public UserReference UserRef
        //{
        //    get
        //    {
        //        return _userRef;
        //    }
        //    set
        //    {
        //        Id = _userRef.Id;
        //        _userRef = value;
        //        _userRef.Id = Id;
        //        _email = value.Email;
        //        _userType = value.UserType;
        //        _cellPhoneNumber = value.CellPhoneNumber;
        //        _gender = value.Gender;
        //        _address = value.Address;
        //        _firstName = value.FirstName;
        //        _lastName = value.LastName;
        //        UserName = value.UserName;
        //    }
        //}
        //private string _email;
        //private string _userType;
        //private string _cellPhoneNumber;
        //private string _gender;
        //private string _address;
        //private string _firstName;
        //private string _lastName;

        //public override string UserName
        //{
        //    get
        //    {
        //        return base.UserName;
        //    }
        //    set
        //    {
        //        base.UserName = value;
        //        UserRef.UserName = value;
        //    }
        //}

        //public override string Email
        //{
        //    get
        //    {
        //        return _email;
        //    }
        //    set
        //    {
        //        _email = value;
        //        UserRef.Email = value;
        //    }
        //}

        //public string UserType
        //{
        //    get
        //    {
        //        return _userType;
        //    }
        //    set
        //    {
        //        _userType = value;
        //        UserRef.UserType = value;
        //    }
        //}

        //public string CellPhoneNumber
        //{
        //    get
        //    {
        //        return _cellPhoneNumber;
        //    }
        //    set
        //    {
        //        _cellPhoneNumber = value;
        //        UserRef.CellPhoneNumber = value;
        //    }
        //}
        //public string Gender
        //{
        //    get
        //    {
        //        return _gender;
        //    }
        //    set
        //    {
        //        _gender = value;
        //        UserRef.Gender = value;
        //    }
        //}
        //public string Address
        //{
        //    get
        //    {
        //        return _address;
        //    }
        //    set
        //    {
        //        _address = value;
        //        UserRef.Address = value;
        //    }
        //}
        //public string FirstName
        //{
        //    get
        //    {
        //        return _firstName;
        //    }
        //    set
        //    {
        //        _firstName = value;
        //        UserRef.FirstName = value;
        //    }
        //}
        //public string LastName
        //{
        //    get
        //    {
        //        return _lastName;
        //    }
        //    set
        //    {
        //        _lastName = value;
        //        UserRef.LastName = value;
        //    }
        //}

    }
}
