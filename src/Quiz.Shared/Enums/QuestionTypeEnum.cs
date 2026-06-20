using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Quiz.Shared.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum QuestionTypeEnum
{
    [EnumMember(Value = "MCQ")]
    MCQ,

    [EnumMember(Value = "MSQ")]
    MSQ,

    [EnumMember(Value = "TF")]
    TF,

    [EnumMember(Value = "SA")]
    SA
}
