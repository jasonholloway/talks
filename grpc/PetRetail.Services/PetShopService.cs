using System.Threading.Tasks;
using Grpc.Core;

namespace PetRetail.Services
{
    public class PetShopService : PetShop.PetShopBase
    {
        private readonly ParrotProvider _provider;
        private readonly KittenBasket _basket;
    
        public PetShopService(ParrotProvider provider, KittenBasket basket)
        {
            _provider = provider;
            _basket = basket;
        }
        
        public override Task<Parrot> BuyParrot(ParrotSpec spec, ServerCallContext _)
        {
            if (spec.Age >= 100)
            {
                throw new ParrotSupplyException();
            }

            return _provider.FindParrot(spec.Breed);
        }
    
        public override async Task OrderCatFood(Amount request, IServerStreamWriter<BoxOfKibbles> boxesOfFood, ServerCallContext _)
        {
            await boxesOfFood.WriteAsync(new BoxOfKibbles { Brand = "Sheba" });
            await boxesOfFood.WriteAsync(new BoxOfKibbles { Brand = "Kit-e-Kat" });
            await boxesOfFood.WriteAsync(new BoxOfKibbles { Brand = "IAMS" });
        }

        public override async Task DonateKittens(IAsyncStreamReader<LitterOfKittens> litters, IServerStreamWriter<CeaseAndDesist> responses, ServerCallContext _)
        {
            await foreach (var litter in litters.ReadAllAsync())
            {
                foreach (var kitten in litter.Kittens)
                {
                    _basket.AddKitten(kitten);
                }

                if (_basket.TooManyKittens())
                {
                    await responses.WriteAsync(new CeaseAndDesist { Severity = 100, Message = "Please stop donating kittens" });
                }
            }
        }
    }
}