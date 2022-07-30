import { Component, OnInit } from '@angular/core';
import { response } from 'express';
import { Card } from './models/card.model';
import { CardsService } from './service/cards.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit{
  title = 'cards';
  cards: Card[] = []; //sağ taraftaki gridi populate ettiğimiz yer 
  card: Card ={
    id: '',
    cardNumber:'',
    cardholderName:'',
    cvc:'',
    expiryMonth:'',
    expiryYear:'',
    
    
  }

  constructor(private cardService:CardsService){
    
  }
  ngOnInit(): void {  //sayfa ilk yüklendiğinde componentleri çağırmamız için kullanıyoruz
    this.getAllCards()
  }

  getAllCards(){
    this.cardService.getAllCards()
    .subscribe(   //subsricbe observable olan get all carddan api nin çağırıldığından emin olmak için kullanıyoruz
        response=>{
          this.cards=response;
        
         
        }
    );
  }

  onSubmit(){

    if(this.card.id===''){
      this.cardService.addcard(this.card)
      .subscribe(
        response=>{
          this.getAllCards() //sayfayı yenilemeden eklenen veri gelsin diye
          this.card= {
            id: '',
            cardNumber:'',
            cardholderName:'',
            expiryMonth:'',
            expiryYear:'',
            cvc:''
          }
        }
      );
    }else{
      this.updateCard(this.card)

    }
    this.clearForm()
    this.ngOnInit()
   


  
  }


  deleteCard(id:string){
    this.cardService.deleteCard(id)
    .subscribe(
      response=>{
        this.getAllCards();
      }
    )
  }

  populateForm(card:Card){
    this.card =card;

    console.log(card)
  }

  clearForm(){
    this.card.id ='';
    this.card.cardNumber ='';
    this.card.cardholderName ='';
    this.card.cvc ='';
    this.card.expiryMonth ='';
    this.card.expiryYear ='';
  }

  updateCard(card:Card){
    this.cardService.updateCard(card)
    .subscribe(
      response=>{
        this.getAllCards()
      }
    )
  }

}
