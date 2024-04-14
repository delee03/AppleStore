if ($(window).width() > 991) {
  $('.details-image').on('afterChange', function (event, slick, currentSlide, nextSlide) {
    var imgs = $('.image_zoom_cls');
    $('.zoomContainer').remove();
    imgs.removeData('elevateZoom');
    imgs.removeData('zoomImage');
    var temp_zoom_cls = '.image_zoom_cls-' + currentSlide;
    setTimeout(function () {
      $(temp_zoom_cls).elevateZoom({
        zoomType: "inner",
        cursor: "crosshair"
      });
    }, 200);
  });
}
if ($(window).width() > 991) {
  setTimeout(function () {
    $('.details-image').elevateZoom({
      zoomType: "inner",
      cursor: "crosshair"
    });
  }, 100);
}