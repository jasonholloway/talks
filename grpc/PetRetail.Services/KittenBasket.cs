namespace PetRetail.Services
{
    public class KittenBasket
    {
        private int _kittenCount = 0;
        
        public void AddKitten(Kitten kitten)
        {
            _kittenCount++;
        }

        public bool TooManyKittens()
        {
            return _kittenCount > 10;
        }
    }
}