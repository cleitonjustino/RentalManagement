using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace RentalManagement.Domain.Enums
{
    public enum TypeLicense
    {
        [Description("A")]
        [Display(Name = "A")]
        [JsonProperty("A")]
        [EnumMember(Value = "A")] 
        A,
        [Description("B")]
        [Display(Name = "B")]
        [JsonProperty("B")]
        [EnumMember(Value = "B")]
        B,
        [Description("AB")]
        [Display(Name = "AB")]
        [JsonProperty("AB")]
        [EnumMember(Value = "AB")]
        AB 
    }
}
