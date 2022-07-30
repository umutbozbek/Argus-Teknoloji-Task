using Cards.API.Data;
using Cards.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Cards.API.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class CardsController : Controller

    //bunun sayesinde cardsın içinde kullanabilirim galiba :D
    {
        private readonly CardsDbContext cardsDbContext;

        public CardsController(CardsDbContext cardsDbContext)
        {
            this.cardsDbContext = cardsDbContext;
        }


        //Get All Cards
        [HttpGet]
        public async Task<IActionResult> GetAllCards()
        {
            var Cards = await cardsDbContext.Cards.ToListAsync();
            return Ok(Cards);
        }

        //Get single Card

        [HttpGet]
        [Route("{id:guid}")]
        [ActionName("GetCard")] 
        public async Task<IActionResult> GetCard([FromRoute] Guid id )
        {
            var Card = await cardsDbContext.Cards.FirstOrDefaultAsync(x=>x.Id==id);
            if (Card != null)
            {
                return Ok(Card);
            }
            return NotFound("Kartınız bulunamadı");
        }

        //Add Card
        [HttpPost]
       
        public async Task<IActionResult> AddCard([FromBody] Card card)
        {
          card.Id = Guid.NewGuid();

            await cardsDbContext.Cards.AddAsync(card);
            await cardsDbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCard), new { id= card.Id } , card);
        }

        //Updating Card
        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateCard( /*refrence to which card  change */[FromRoute] Guid id, /* updated details*/[FromBody] Card card)
        {
            var existingCard = await cardsDbContext.Cards.FirstOrDefaultAsync(x => x.Id == id);
            if(existingCard != null)
            {
                existingCard.CardholderName = card.CardholderName;
                existingCard.CardNumber = card.CardNumber;
                existingCard.ExpiryMonth = card.ExpiryMonth;
                existingCard.ExpiryYear = card.ExpiryYear;
                existingCard.CVC = card.CVC;
                await cardsDbContext.SaveChangesAsync(); /*önceden bildiği için değişiklikleri kaydedelim */
                return Ok(existingCard);
            }
            return NotFound("Kartınız bulunamadı");
        }

        //Delete Card
        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteCard([FromRoute] Guid id)
        {
            var existingCard = await cardsDbContext.Cards.FirstOrDefaultAsync(x => x.Id == id);
            if (existingCard != null)
            {
                cardsDbContext.Remove(existingCard);
                await cardsDbContext.SaveChangesAsync();
                return Ok(existingCard);
            }
            return NotFound("Kartınız bulunamadı");
        }

    }
}
