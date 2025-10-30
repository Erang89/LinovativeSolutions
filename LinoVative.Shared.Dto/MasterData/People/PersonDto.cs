using Linovative.Shared.Interface.Enums;
using LinoVative.Shared.Dto;
using LinoVative.Shared.Dto.Attributes;

namespace Linovative.Dto.MasterData.People
{
    [LocalizerKey(nameof(PersonDto))]
    public class PersonDto : EntityDtoBase
    {
        [LocalizedRequired]
        public string? Firstname { get; set; }
        public string? Lastname { get; set; }
        [LocalizedRequired]
        public PersonTitles? Title { get; set; }
        public string? Nikname { get; set; }
        [LocalizedRequired, LocalizedPhone, UniqueField(EntityTypes.Person, fieldName: nameof(PhoneNumber))]
        public string? PhoneNumber { get; set; }
    }

    public class PersonViewDto : PersonDto
    {
        public string? Name => $"{Firstname} {Lastname}";
    }

}
