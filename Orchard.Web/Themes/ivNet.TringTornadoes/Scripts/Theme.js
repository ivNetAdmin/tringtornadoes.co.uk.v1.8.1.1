function startSlideshow(){
    $("img.sponsor1").fadeIn(1000).delay(1050).fadeOut(10500); //13000
    $("img.sponsor2").delay(13000).fadeIn(1500).delay(11000).fadeOut(1500); //27000
    $("img.sponsor3").delay(27000).fadeIn(1500).delay(11000).fadeOut(1500); //41000
    $("img.sponsor4").delay(41000).fadeIn(1500).delay(11000).fadeOut(1500); //55000
    $("img.sponsor5").delay(55000).fadeIn(1500).delay(11000).fadeOut(1500); //69000
    $("img.sponsor6").delay(69000).fadeIn(1500).delay(11000).fadeOut(1500, startSlideshow); //83000
}

$(document).ready(function(){
    startSlideshow();
});
