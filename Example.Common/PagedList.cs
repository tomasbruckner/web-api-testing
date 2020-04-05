using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Example.Common
{
    public class PagedList<T>
    {
        public PagedList() : this(0, new List<T>())
        {
        }

        public PagedList(T data) : this(new List<T> {data})
        {
        }

        public PagedList(ICollection<T> data) : this(data.Count, data)
        {
        }

        public PagedList(int size, Task<ICollection<T>> data) : this(size, data.Result)
        {
        }

        public PagedList(int size, IEnumerable<T> data) : this(size, data.ToList())
        {
        }

        public PagedList(int size, ICollection<T> data, string status = "ok")
        {
            Size = size;
            Data = data;
            Status = status;
        }

        public int Size { get; set; }
        public ICollection<T> Data { get; set; }
        public string Status { get; set; }
    }
}
