using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Test1.Models;

namespace Test1
{
    public class ItemType
    {
        public enum Types {Sword, Axe, Shield, Armor, Potion };
    }

    
    public class ItemTypeAttribute : ValidationAttribute
    {
        Item _item;
        int _swordmin = 3;

        public ItemTypeAttribute(Item item)
        {
            _item = item;
        }
        private string GetErrorMessage()
        {
            return $"You must have a level of at least {_swordmin} to use swords.";
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            Item item = (Item)validationContext.ObjectInstance;
            if (item.Type == ItemType.Types.Sword && item.Level < 3)
            {
                return new ValidationResult(GetErrorMessage());
            }
            return ValidationResult.Success;



        }
    }
}
