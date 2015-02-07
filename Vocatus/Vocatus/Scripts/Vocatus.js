
// javascript functions

function addMixer() {
    var e = document.getElementById('RemainingMixers');
    var id = e.options[e.selectedIndex].value;
    addIngredient(id);
}

function addLiquor() {
    var e = document.getElementById('RemainingLiquors');
    var id = e.options[e.selectedIndex].value;
    addIngredient(id);
}

function addLiqueur() {
    var e = document.getElementById('RemainingLiqueurs');
    var id = e.options[e.selectedIndex].value;
    addIngredient(id);
}

function addCordial() {
    var e = document.getElementById('RemainingCordials');
    var id = e.options[e.selectedIndex].value;
    addIngredient(id);
}

function addMisc() {
    var e = document.getElementById('RemainingMisc');
    var id = e.options[e.selectedIndex].value;
    addIngredient(id);
}

function addWine() {
    var e = document.getElementById('RemainingWines');
    var id = e.options[e.selectedIndex].value;
    addIngredient(id);
}

// universal ingredient add, ajax call
function addIngredient(id) {

    $.ajax({
        url: '/Drink/InsertIngredientForUser',
        data: 'ingredientID='+id,
        contentType: 'application/html; charset=utf-8'

    }).success(function (result) {
        location.reload();
    }).error(function (xhr, status) {
        alert("Error inserting");
     });

}

function removeIngredient(id) {
    $.ajax({
        url: '/Drink/RemoveIngredientForUser',
        data: 'ingredientID=' + id,
        contentType: 'application/html; charset=utf-8'

    }).success(function (result) {
        location.reload();
    }).error(function (xhr, status) {
        alert("Error removing");
    });
}