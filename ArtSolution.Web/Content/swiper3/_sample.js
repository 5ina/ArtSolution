var
	swiperMainSliderOptions = {
		loop: true,
		pagination: '.swiper-pagination',
		paginationClickable: true,
		effect: 'fade',
		speed: 3000,
		autoplay: 9000,
		autoplayDisableOnInteraction: false,
		spaceBetween: 10,
	},
	swiperVertSliderOptions = {
		loop: true,
		pagination: '.swiper-pagination',
		paginationClickable: true,
		effect: 'slide',
		speed: 1500,
		autoplay: 6000,
		autoplayDisableOnInteraction: false,
		spaceBetween: 10,
	},
	swiperHorMenuOptions = {
		loop: false,
		prevButton: '.swiper-button-prev',
		nextButton: '.swiper-button-next',
		slidesPerView: 'auto',
		effect: 'slide',
		speed: 400,
		observer: true,
		observeParents: true,
		initialSlide: 0,
	};


(function ($) {

	$('.swiper-container.main-slider').each(function () {
		var sw = $(this).swiper(swiperMainSliderOptions);
	});


	$('.swiper-container.vert-slider').each(function () {
		var sw = $(this).swiper(swiperVertSliderOptions);
	});


	$('.swiper-container.hor-menu').each(function () {
		var sw = $(this).swiper(swiperHorMenuOptions);
	});

})(jQuery);