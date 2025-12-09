document.addEventListener('DOMContentLoaded', () => {

	new WOW().init();

	// Menú móvil
	$('.hamburger').on('click', function() {
		if($(this).hasClass('is-active')) {
			$(this).removeClass('is-active');
			$('.header-mobile-wrap').slideUp(500);
		}
		else {
			$(this).addClass('is-active');
			$('.header-mobile-wrap').slideDown(500);
		}
	});

	// scroll
	function scrollHeader() {
		let headerTopHeight = $('.header-top').height();
		if($(this).scrollTop() > headerTopHeight) {
			$('.header-bottom').addClass('is-fixed');
		}
		else {
			$('.header-bottom').removeClass('is-fixed');
		}
	}

	// Mostrar botón para subir
	function showArrowUp() {
		if($(this).scrollTop() > 1500) {
			$('.go-up').addClass('is-active');
		}
		else {
			$('.go-up').removeClass('is-active');
		}
	}

	// Contador animado
	function animateCounter(element, targetValue, duration) {
		jQuery({ count: jQuery(element).text() }).animate(
			{
				count: targetValue
			},
			{
				duration: duration,
				easing: 'linear',
				step: function () {
					jQuery(element).text(Math.floor(this.count));
				},
				complete: function () {
					jQuery(element).text(targetValue);
				},
			}
		);
	}

	function isElementInViewport(elem) {
		if (!elem) return false;
		var rect = elem.getBoundingClientRect();
		return (
			rect.top >= 0 &&
			rect.left >= 0 &&
			rect.bottom <= (window.innerHeight || document.documentElement.clientHeight) &&
			rect.right <= (window.innerWidth || document.documentElement.clientWidth)
		);
	}

	jQuery(window).on('scroll', function () {
		if (isElementInViewport(jQuery('.num-scroll')[0])) {
			jQuery('.num-js').each(function () {
				var targetValue = parseInt(jQuery(this).data('count'));
				var duration = 2000;
				animateCounter(this, targetValue, duration);
			});

			jQuery(window).off('scroll');
		}
	});

	jQuery(window).trigger('scroll');

	// Eventos de scroll
	$(window).on('scroll', function() {
		scrollHeader();
		showArrowUp();
	});
	scrollHeader();
	showArrowUp();

	// Scroll suave para enlaces ancla
	$('.anchor-link').on('click', function () {
	    let href = $(this).attr('href');

	    $('html, body').animate({
	        scrollTop: $(href).offset().top
	    }, {
	        duration: 700,
	    });
		$('.header-mobile-wrap').slideUp(500);
		$('.hamburger').removeClass('is-active');
	    return false;
	});

	// Botón subir arriba
	$('.go-up').on('click', function () {
	    $('html, body').animate({
	        scrollTop: 0
	    }, {
	        duration: 700,
	    });
	    return false;
	});

	// Slider del banner
	const bannerSwiper = new Swiper('.banner-swiper', {
		speed: 1000,
		spaceBetween: 0,
		autoHeight: true,
		navigation: {
			nextEl: '.banner-swiper .swiper-button-next',
			prevEl: '.banner-swiper .swiper-button-prev',
		},
		pagination: {
			el: '.banner-swiper .swiper-pagination',
			type: 'bullets',
			clickable: true,
		},
	});

	// Servicios
	$('.services-btn').magnificPopup({
		type: 'inline',
		showCloseBtn: false,
		removalDelay: 500,
		callbacks: {
			beforeOpen: function() {
			   this.st.mainClass = this.st.el.attr('data-effect');
			}
		},
	});

	$('.modal-form-close').on('click', function() {
		$.magnificPopup.close();
	});

	// Galería de imágenes
	$('.gallery-wrap a').magnificPopup({
		type: 'image',
		gallery: {
			enabled: true
		},
		callbacks: {
			beforeOpen: function() {
				this.st.image.markup = this.st.image.markup.replace('mfp-figure', 'mfp-figure mfp-with-anim');
				this.st.mainClass = this.st.el.attr('data-effect');
			}
		},
	});

	// Botón mostrar más galería
	$('.gallery-btn a').on('click', function(e) {
		e.preventDefault();
		var galleryItem = $('.gallery-item');

		if($(this).hasClass('is-active')) {
			$(this).removeClass('is-active');
			$(this).text('Show more');
			galleryItem.each(function() {
				if($(this).hasClass('is-active')) {
					$(this).removeClass('is-active');
					$(this).slideUp();
				}
			});
		}
		else {
			$(this).addClass('is-active');
			$(this).text('Hide');
			galleryItem.each(function() {
				if(!$(this).is(':visible')) {
					$(this).addClass('is-active');
					$(this).slideDown();
				}
			});
		}
	});

	// Slider de reseñas
	const reviewsSwiper = new Swiper('.reviews-swiper', {
		speed: 1000,
		spaceBetween: 20,
		pagination: {
			el: '.reviews-swiper .swiper-pagination',
			type: 'bullets',
			clickable: true,
		},
		breakpoints: {
			320: {
				slidesPerView: 1,
			},
			575: {
				slidesPerView: 2,
			},
			992: {
				slidesPerView: 3,
			},
		}
	});

	// Configuración de EmailJS
	const EMAILJS_SERVICE_ID = 'service_k7lz71j';
	const EMAILJS_TEMPLATE_ID = 'template_9r0hpdi';

	// Función para enviar email
	async function enviarEmail(formData) {
		const templateParams = {
			from_name: formData.get('nombre'),
			from_phone: formData.get('celular'),
			from_email: formData.get('correo'),
			appointment_date: formData.get('fecha'),
			appointment_time: formData.get('hora'),
			message: formData.get('comentarios') || 'Sin comentarios'
		};

		console.log('Enviando datos:', templateParams);

		try {
			const response = await emailjs.send(
				EMAILJS_SERVICE_ID,
				EMAILJS_TEMPLATE_ID,
				templateParams
			);

			console.log('Email enviado exitosamente:', response);
			alert('¡Cita agendada exitosamente! Te hemos enviado un correo de confirmación.');
			return true;

		} catch (error) {
			console.error('Error completo:', error);
			console.error('Detalles del error:', error.text || error.message);
			alert('Hubo un error al agendar la cita. Por favor, intenta de nuevo o llámanos al +507 6641-4004');
			return false;
		}
	}

	// Formulario
	$('.modal-form form').on('submit', async function(e) {
		e.preventDefault();
		console.log('Formulario modal enviado');
		const formData = new FormData(this);
		const success = await enviarEmail(formData);
		if (success) {
			this.reset();
			$.magnificPopup.close();
		}
	});

	// Formulario de contacto
	$('.s-form form').on('submit', async function(e) {
		e.preventDefault();
		console.log('Formulario contacto enviado');
		const formData = new FormData(this);
		const success = await enviarEmail(formData);
		if (success) {
			this.reset();
		}
	});

	// Establecer fecha mínima como hoy
    document.addEventListener('DOMContentLoaded', function() {
        const hoy = new Date().toISOString().split('T')[0];
        document.getElementById('fechaCitaModal').setAttribute('min', hoy);
        document.getElementById('fechaCitaContacto').setAttribute('min', hoy);
    });

})