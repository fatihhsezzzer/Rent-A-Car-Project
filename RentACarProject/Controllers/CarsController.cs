using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using RentACar.Models.CarModels;
using RentACar.Data;
using RentACarProject.Models.CarModels;
using Iyzipay;
using Iyzipay.Request;
using Iyzipay.Model;
using Newtonsoft.Json;

public class CarsController : Controller
{
    private readonly ApplicationDbContext _context;

    public CarsController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: Cars
    [HttpGet]
    public IActionResult CarList()
    {
        var cars = _context.Cars.ToList();
        return View(cars);
    }

    // GET: Cars/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var car = await _context.Cars
            .FirstOrDefaultAsync(m => m.Id == id);
        if (car == null)
        {
            return NotFound();
        }

        return View(car);
    }

    // GET: Cars/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: Cars/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,BodyType,SeatCount,DoorCount,LuggageCapacity,FuelType,EngineSize,Year,Mileage,Transmission,DriveType,FuelEconomy,ExteriorColor,InteriorColor")] Car car)
    {
        if (ModelState.IsValid)
        {
            _context.Add(car);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(car);
    }

    // GET: Cars/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var car = await _context.Cars.FindAsync(id);
        if (car == null)
        {
            return NotFound();
        }
        return View(car);
    }

    // POST: Cars/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,BodyType,SeatCount,DoorCount,LuggageCapacity,FuelType,EngineSize,Year,Mileage,Transmission,DriveType,FuelEconomy,ExteriorColor,InteriorColor")] Car car)
    {
        if (id != car.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(car);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CarExists(car.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
        }
        return View(car);
    }

    // GET: Cars/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var car = await _context.Cars
            .FirstOrDefaultAsync(m => m.Id == id);
        if (car == null)
        {
            return NotFound();
        }

        return View(car);
    }

    // POST: Cars/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var car = await _context.Cars.FindAsync(id);
        _context.Cars.Remove(car);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool CarExists(int id)
    {
        return _context.Cars.Any(e => e.Id == id);
    }
    [HttpPost]
    public async Task<IActionResult> FilterCars(CarFilterModel filter)
    {
        var filteredCars = await _context.Cars
            // Burada filtreleme koşullarınızı uygulayın
            .Where(car =>( filter.MaxPrice==null|| car.PricePerDay <= filter.MaxPrice) && (filter.Location==null|| car.Location == filter.Location )&& (filter.Passengers == null || car.Passengers == filter.Passengers )&& (filter.FuelType==null|| car.FuelType == filter.FuelType))
            .ToListAsync();

        return View("CarList", filteredCars); // Filtrelenmiş arabaları içeren bir view'a dönüş yapın
    }
    [HttpPost]
    public IActionResult MakeReservation(ReservationModel model)
    {
        if (!ModelState.IsValid)
        {
            // Toplam maliyeti hesaplayın
            decimal totalCost = model.Days * model.PricePerDay;

            // Hesaplanan toplam maliyeti ve rezervasyon detaylarını yeni modele atayın
            var resultModel = new ReservationResultModel
            {
                TotalCost = totalCost,
               
            };

            // TempData'ya sonucu kaydedin
            TempData["ReservationResult"] = JsonConvert.SerializeObject(resultModel);

            // Başka bir action'a yönlendirme yapın
            return RedirectToAction("Order");
        }

        // Model geçerli değilse, formu tekrar göster
        return View();
    }

    public IActionResult Order()
    {
        if (TempData.TryGetValue("ReservationResult", out var resultJson))
        {
            var resultModel = JsonConvert.DeserializeObject<ReservationResultModel>(resultJson.ToString());

            Options options = new Options(); // Iyzico Import
            options.ApiKey = "sandbox-WiB0cOxTccLqUfpbtk7Tuf26CQcoz1LC";
            options.SecretKey = "sandbox-adlGN5zVPcimQIeFnXR4VgJI14MSXzRV";
            options.BaseUrl = "Https://sandbox-api.iyzipay.com";

            //double savePrice = 0;
            //double delivaryShippingPrice = 38;
            //foreach (var item in GetCart().CartLines)
            //{
            //    savePrice += ((item.Quantity * item.Advert.Price) / 100) * campaignRepository.Detail(item.Advert.CampaignId).Rate;
            //}
            //double Price = GetCart().TotalPrice() - savePrice + delivaryShippingPrice;
            //string TotalPrice = Math.Round(Price, 2).ToString().Replace(',', '.');

            //var user = userRepository.UserAccount(User.Identity.Name);
            //var userShippingAddress = addressRepository.List().FirstOrDefault(x => x.UserId == user.Id && x.IsSelected == true);
            //var userProvince = provinceRepository.Detail(userShippingAddress.ProvinceId);

            var fiyat = Convert.ToString(resultModel.TotalCost);

            CreateCheckoutFormInitializeRequest request = new CreateCheckoutFormInitializeRequest();
            request.Locale = Locale.TR.ToString();
            request.ConversationId = "123456789";
            request.Price = fiyat;
            request.PaidPrice = fiyat;
            request.Currency = Currency.TRY.ToString();
            request.BasketId = "B67832";
            request.PaymentGroup = PaymentGroup.PRODUCT.ToString();
            request.CallbackUrl = "https://localhost:7122/Cars/Success";

            //List<int> enabledInstallments = new List<int>();
            //enabledInstallments.Add(2);
            //enabledInstallments.Add(3);
            //enabledInstallments.Add(6);
            //enabledInstallments.Add(9);
            //request.EnabledInstallments = enabledInstallments;

            Buyer buyer = new Buyer();
            buyer.Id = "asdadsada";
            buyer.Name = "Erhan";
            buyer.Surname = "Kaya";
            buyer.GsmNumber = "+905554443322";
            buyer.Email = "email@email.com";
            buyer.IdentityNumber = "74300864791";
            buyer.LastLoginDate = "2015-10-05 12:43:35";
            buyer.RegistrationDate = "2000-12-12 12:00:00";
            buyer.RegistrationAddress = "Nidakule Göztepe, Merdivenköy Mah. Bora Sok. No:1";
            buyer.Ip = "85.34.78.112";
            buyer.City = "Istanbul";
            buyer.Country = "Turkey";
            buyer.ZipCode = "34732";
            request.Buyer = buyer;

            Address shippingAddress = new Address();
            shippingAddress.ContactName = "Erhan Kaya";
            shippingAddress.City = "Istanbul";
            shippingAddress.Country = "Turkey";
            shippingAddress.Description = "Bereket döner karşısı";
            shippingAddress.ZipCode = "34742";
            request.ShippingAddress = shippingAddress;

            Address billingAddress = new Address();
            billingAddress.ContactName = "Erhan Kaya";
            billingAddress.City = "Istanbul";
            billingAddress.Country = "Turkey";
            billingAddress.Description = "Bereket Döner";
            billingAddress.ZipCode = "34742";
            request.BillingAddress = billingAddress;

            List<BasketItem> basketItems = new List<BasketItem>();
            BasketItem basketProduct;
            basketProduct = new BasketItem();
            basketProduct.Id = "1";
            basketProduct.Name = "Asus Bilgisayar";
            basketProduct.Category1 = "Bilgisayar";
            basketProduct.Category2 = "";
            basketProduct.ItemType = BasketItemType.PHYSICAL.ToString();

            double price = 1;
            double endPrice = 1;
            basketProduct.Price = fiyat;
            basketItems.Add(basketProduct);

            request.BasketItems = basketItems;

            CheckoutFormInitialize checkoutFormInitialize = CheckoutFormInitialize.Create(request, options);
            ViewBag.pay = checkoutFormInitialize.CheckoutFormContent;
            return View();
        }
        return View("Details", resultJson);

    }

    public IActionResult Success()
    {
        return View();
    }
}
