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
        A = 1,
        [Description("B")]
        [Display(Name = "B")]
        [JsonProperty("B")]
        [EnumMember(Value = "B")]
        B = 2,
        [Description("AB")]
        [Display(Name = "AB")]
        [JsonProperty("AB")]
        [EnumMember(Value = "AB")]
        AB = 3
    }
}
