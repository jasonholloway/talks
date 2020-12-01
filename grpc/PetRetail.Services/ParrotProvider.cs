using System.Threading.Tasks;

namespace PetRetail.Services
{
    public class ParrotProvider
    {
        public async Task<Parrot> FindParrot(ParrotBreed breed)
        {
            await Task.Delay(300);

            switch (breed)
            {
                case ParrotBreed.AfricanGrey:
                    return new Parrot
                    {
                        Name = "Agnes", 
                        Breed = ParrotBreed.AfricanGrey,
                        Price = 500
                    };
                
                case ParrotBreed.ScarletMacaw:
                    return new Parrot
                    {
                        Name = "Maurice", 
                        Breed = ParrotBreed.ScarletMacaw,
                        Price = 400
                    };
                
                default:
                    throw new ParrotSupplyException();
            }
        }
    }
}