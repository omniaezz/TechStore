//document.getElementById('button1').addEventListener('click', function() {
//    document.getElementById('button1').classList.add('hidden');
//    document.getElementById('button2').classList.remove('hidden');
//  });
  
//  document.getElementById('button2').addEventListener('click', function() {
//    document.getElementById('button2').classList.add('hidden');
//    document.getElementById('button1').classList.remove('hidden');
//  });
 

  ////////modal//////////////
var modal = document.getElementById("confirmationModal");


var removeButton = document.querySelector(".Remove");


var cancelDeleteBtn = document.getElementById("cancelDelete");

document.querySelectorAll(".Remove").forEach(btn =>{
btn.addEventListener('click', function(){
  modal.style.display = "block";
 document.querySelector('#confirmDelete').addEventListener('click',
 
 function(){
  var productprice = document.getElementById('productprices').textContent;

    var myCartApp = JSON.parse(localStorage.getItem('mycartapp'));
 console.log(productprice)

    var index = myCartApp.findIndex(item => item.price === productprice);
    
    if (index !== -1) {
        myCartApp.splice(index, 1);
    }
    
  
    localStorage.setItem('mycartapp', JSON.stringify(myCartApp));

    location.reload();
 

 }
 )
 
})

})
//cancelDeleteBtn.onclick = function() {
//  modal.style.display = "none";
//}


window.onclick = function(event) {
  if (event.target == modal) {
    modal.style.display = "none";
  }
}



//////////Add Item////////////////////////
function addToCart(item) {

  let cart = JSON.parse(localStorage.getItem('mycartapp')) || [];
console.log(item)
  
  cart.push(item);


  localStorage.setItem('mycartapp', JSON.stringify(cart));
  location.reload();
}


function removeFromCart(index) {
 
  let cart = JSON.parse(localStorage.getItem('mycartapp')) || [];

  
  cart.splice(index, 1);

  
  localStorage.setItem('mycartapp', JSON.stringify(cart));
 
}

//header///

//var cartLink = document.getElementById('cart_be');


//var cartItems = JSON.parse(localStorage.getItem('mycartapp'));


//var bodyCart = document.getElementById('body_cart');
//var myCart = document.getElementById('mycart');


//if (!cartItems || cartItems.length === 0) {
//    bodyCart.classList.remove('hidden');
//    myCart.classList.add('hidden');
//} else {
    
//    bodyCart.classList.add('hidden');
//    myCart.classList.remove('hidden');
//}


//cartLink.addEventListener('click', function() {
  
//    var cartItems = JSON.parse(localStorage.getItem('mycartapp'));
   
//    if (!cartItems || cartItems.length === 0) {
//        bodyCart.classList.remove('hidden');
//        myCart.classList.add('hidden');
//    } else {
//        bodyCart.classList.add('hidden');
//        myCart.classList.remove('hidden');
//    }

//});

window.onload = function() {
 
  let cart = JSON.parse(localStorage.getItem('mycartapp')) || [];

  
  cart.forEach(function(item, index) {
    console.log("Item: ",item ,index);
   
  });
}







 /*localStorage.setItem('mycartapp', JSON.stringify(null)); */

//Relod page////////////////
window.onload = function() {
  var myCart = document.getElementById('mycart'); 
  var bodyCart = document.getElementById('body_cart'); 
  var cartItems = JSON.parse(localStorage.getItem('mycartapp'));
  
  if (cartItems && cartItems.length > 0) {
    // bodyCart.classList.add('hidden');
        myCart.classList.remove('hidden');
  } else {
    bodyCart.classList.remove('hidden');
    myCart.classList.add('hidden');
  }
};

//button bodycart////
document.querySelectorAll(".btn").forEach(btn => {
  btn.addEventListener('click', function(event) {
      var clickedButton = event.currentTarget;
     
      var parentDiv = clickedButton.closest('.card');
      var img = parentDiv.querySelector('.card-img-top').getAttribute('src');
      var text = parentDiv.querySelector('.card-text').textContent;
      var price = parentDiv.querySelector('.cart-price').textContent;

      var myCart = document.getElementById('mycart');
      var bodyCart = document.getElementById('body_cart'); 

      var cartItems = JSON.parse(localStorage.getItem('mycartapp'));

      if (cartItems === null || cartItems.length === 0) {
          myCart.classList.add('hidden');
      } else {
          myCart.classList.remove('hidden');
          bodyCart.classList.add('hidden');
      }

      var product = {
          img: img,
          text: text,
          price: price
      };
      addToCart(product);
     
  });
});
function toggleDescription() {
    var description = document.getElementById('productDescription');
    if (description.style.display === 'none') {
        description.style.display = 'block';
    } else {
        description.style.display = 'none';
    }
}

//var quantityDropdown = document.getElementById("quantityDropdown");

//quantityDropdown.addEventListener("change", function () {
//    document.getElementById("updateQuantityForm").submit();
//});

//$(document).ready(function () {
//    $('#quantityDropdown').hide();

//    $('#dropdownIcon').click(function () {
//        $('#quantityDropdown').toggle();
//    });
//});

//// var cartItems = JSON.parse(localStorage.getItem('mycartapp')) || [];

//document.addEventListener("DOMContentLoaded", function () {
//    var quantityDropdown = document.getElementById("quantityDropdown");
//    var pricePerItem = parseFloat(document.getElementById("itemPrice").innerText); // Assuming there's an element displaying the price per item
//    var totalPriceDisplay = document.getElementById("totalPriceDisplay");

//    // Initial calculation and display of total price
//    var updateTotalPrice = function () {
//        var quantity = parseInt(quantityDropdown.value);
//        var totalPrice = quantity * pricePerItem;
//        totalPriceDisplay.innerText = totalPrice.toLocaleString(); // Update the displayed total price with formatted number
//    };

//    // Call the function to initially display the total price
//    updateTotalPrice();

//    // Event listener to update total price when quantity changes
//    quantityDropdown.addEventListener("change", function () {
//        updateTotalPrice(); // Update the total price when quantity changes
//    });
//});

