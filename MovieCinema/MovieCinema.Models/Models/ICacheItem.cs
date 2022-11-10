namespace MovieCinema.Models.Models
{
    public interface ICacheItem<TKey>
    {
        public TKey GetKey();
    }
}