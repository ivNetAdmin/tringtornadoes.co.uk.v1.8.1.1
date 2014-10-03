function startSlideshow() {  
    $("img.sponsor1").fadeIn(1000).delay(1050).fadeOut(10500); //13000
    $("img.sponsor2").delay(13000).fadeIn(1500).delay(11000).fadeOut(1500); //27000
    $("img.sponsor3").delay(27000).fadeIn(1500).delay(11000).fadeOut(1500); //41000
    $("img.sponsor4").delay(41000).fadeIn(1500).delay(11000).fadeOut(1500); //55000
    $("img.sponsor5").delay(55000).fadeIn(1500).delay(11000).fadeOut(1500); //69000
    $("img.sponsor6").delay(69000).fadeIn(1500).delay(11000).fadeOut(1500, startSlideshow); //83000
}

function carouselSlider() {
    $('#slider1').revolution({
        dottedOverlay: "none",
        delay: 9000,
        startwidth: 1170,
        startheight: 400,
        hideThumbs: 200,

        thumbWidth: 100,
        thumbHeight: 50,
        thumbAmount: 4,

        navigationType: "both",
        navigationArrows: "solo",
        navigationStyle: "round",

        touchenabled: "on",
        onHoverStop: "on",

        navigationHAlign: "center",
        navigationVAlign: "bottom",
        navigationHOffset: 0,
        navigationVOffset: 20,

        soloArrowLeftHalign: "left",
        soloArrowLeftValign: "center",
        soloArrowLeftHOffset: 0,
        soloArrowLeftVOffset: 0,

        soloArrowRightHalign: "right",
        soloArrowRightValign: "center",
        soloArrowRightHOffset: 0,
        soloArrowRightVOffset: 0,

        shadow: 0,
        fullWidth: "on",
        fullScreen: "off",

        stopLoop: "off",
        stopAfterLoops: -1,
        stopAtSlide: -1,

        shuffle: "off",

        autoHeight: "off",
        forceFullWidth: "off",

        hideThumbsOnMobile: "off",
        hideBulletsOnMobile: "off",
        hideArrowsOnMobile: "off",
        hideThumbsUnderResolution: 0,

        hideSliderAtLimit: 0,
        hideCaptionAtLimit: 0,
        hideAllCaptionAtLilmit: 0,
        startWithSlide: 0,
        fullScreenOffsetContainer: ""
    });
}

$(document).ready(function () {
    carouselSlider();
    $("div.sponsor-images img").hide();
    startSlideshow();
});
