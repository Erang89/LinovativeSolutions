using Linovative.Shared.Interface;
using LinoVative.Shared.Dto.MasterData.Shifts;
using LinoVative.Shared.Dto.Outlets;

namespace Linovative.Frontend.Shared.Extensions
{
    public static class SequenceExtensions
    {
        public static void SortObject<T, U>(this IEnumerable<KeyValuePair<T, U>> data, bool moveUp, object? selectedObject) where U : IHasSequence
        {
            if (selectedObject is not U obj) return;


            if (moveUp && obj.Sequence > 1)
            {
                obj.Sequence--;

                var otherShift = data.Select(x => x.Value).FirstOrDefault(x => !x.Equals(obj) && x.Sequence == obj.Sequence);
                if (otherShift is not null)
                    otherShift.Sequence++;
                return;
            }

            if (!moveUp && data.Where(x => !x.Equals(obj)).Max(x => x.Value.Sequence) > obj.Sequence)
            {
                obj.Sequence++;

                var otherShift = data.Select(x => x.Value).FirstOrDefault(x => !x.Equals(obj) && x.Sequence == obj.Sequence);
                if (otherShift is not null)
                    otherShift.Sequence--;
            }
        }


        public static void ReAssignSequence<T, U>(this IEnumerable<KeyValuePair<T, U>> data) where U : IHasSequence
        {
            var seq = 1;
            foreach (var record in data.OrderBy(x => x.Value.Sequence).Select(x => x.Value))
            {
                record.Sequence = seq;
                seq++;
            }
        }
    }
}
