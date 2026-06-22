using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Quiz.Shared.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum RoleEnum
{
    [EnumMember(Value = "TEACHER")]
    TEACHER,

    [EnumMember(Value = "STUDENT")]
    STUDENT
}
