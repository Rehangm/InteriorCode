/* Home page: hero fade-on-scroll and testimonial slider enhancements */
(function () {
    'use strict';

    var hero = document.querySelector('.hero__content');
    var reduced = window.matchMedia('(prefers-reduced-motion: reduce)').matches;

    if (hero && !reduced) {
        window.addEventListener('scroll', function () {
            var y = window.scrollY;
            if (y > window.innerHeight) return;
            hero.style.transform = 'translateY(' + (y * 0.25) + 'px)';
            hero.style.opacity = String(Math.max(1 - y / (window.innerHeight * 0.8), 0));
        }, { passive: true });
    }

    // Testimonial slider: pause when off-screen, keyboard arrows when focused.
    var carouselEl = document.getElementById('testimonialCarousel');
    if (carouselEl && window.bootstrap) {
        var carousel = window.bootstrap.Carousel.getOrCreateInstance(carouselEl, { interval: 6500, pause: 'hover', touch: true });

        if ('IntersectionObserver' in window) {
            new IntersectionObserver(function (entries) {
                entries.forEach(function (entry) {
                    if (entry.isIntersecting) carousel.cycle(); else carousel.pause();
                });
            }, { threshold: 0.3 }).observe(carouselEl);
        }

        carouselEl.setAttribute('tabindex', '0');
        carouselEl.setAttribute('aria-roledescription', 'carousel');
    }
})();
