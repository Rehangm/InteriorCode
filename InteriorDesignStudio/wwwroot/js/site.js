/* =========================================================================
   Interior Design Studio – global interactions (vanilla JS, no dependencies)
   ========================================================================= */
(function () {
    'use strict';

    var FALLBACK_IMG = '/images/placeholder.svg';
    var prefersReducedMotion = window.matchMedia('(prefers-reduced-motion: reduce)').matches;

    /* ---------- Image fallback (listener lives in <head>; this catches anything already broken) ---------- */
    document.querySelectorAll('img').forEach(function (img) {
        if (img.complete && img.naturalWidth === 0 && !img.dataset.fallbackApplied && img.src.indexOf('data:') !== 0) {
            img.dataset.fallbackApplied = 'true';
            img.removeAttribute('srcset');
            img.src = FALLBACK_IMG;
            img.classList.add('img-fallback');
        }
    });

    /* ---------- Page loader ---------- */
    var loader = document.getElementById('pageLoader');
    function hideLoader() { if (loader) loader.classList.add('is-hidden'); }
    if (document.readyState === 'loading') {
        document.addEventListener('DOMContentLoaded', function () { setTimeout(hideLoader, 250); });
    } else {
        setTimeout(hideLoader, 250);
    }
    setTimeout(hideLoader, 2500); // safety net

    /* ---------- Header: transparent over hero, solid on scroll / when menu is open ---------- */
    var header = document.getElementById('siteHeader');
    var mainNav = document.getElementById('mainNav');
    var backToTop = document.getElementById('backToTop');
    var menuOpen = false;

    function updateHeader() {
        var y = window.scrollY || window.pageYOffset;
        if (header) {
            header.classList.toggle('is-scrolled', y > 40);
            header.classList.toggle('is-solid', y > 40 || menuOpen);
        }
        if (backToTop) backToTop.classList.toggle('is-visible', y > 600);
    }

    var ticking = false;
    window.addEventListener('scroll', function () {
        if (!ticking) {
            window.requestAnimationFrame(function () { updateHeader(); ticking = false; });
            ticking = true;
        }
    }, { passive: true });
    updateHeader();

    if (mainNav) {
        mainNav.addEventListener('show.bs.collapse', function () { menuOpen = true; updateHeader(); });
        mainNav.addEventListener('hidden.bs.collapse', function () { menuOpen = false; updateHeader(); });

        // Close the mobile menu after a link is chosen.
        mainNav.querySelectorAll('a').forEach(function (link) {
            link.addEventListener('click', function () {
                if (window.bootstrap && mainNav.classList.contains('show')) {
                    window.bootstrap.Collapse.getOrCreateInstance(mainNav).hide();
                }
            });
        });

        // Close the menu with Escape.
        document.addEventListener('keydown', function (e) {
            if (e.key === 'Escape' && mainNav.classList.contains('show') && window.bootstrap) {
                window.bootstrap.Collapse.getOrCreateInstance(mainNav).hide();
            }
        });
    }

    /* ---------- Back to top ---------- */
    function scrollToTop(e) {
        if (e) e.preventDefault();
        window.scrollTo({ top: 0, behavior: prefersReducedMotion ? 'auto' : 'smooth' });
    }
    if (backToTop) backToTop.addEventListener('click', scrollToTop);
    document.querySelectorAll('.js-scroll-top').forEach(function (el) { el.addEventListener('click', scrollToTop); });

    /* ---------- Smooth scrolling for same-page anchors ---------- */
    document.addEventListener('click', function (e) {
        var link = e.target.closest('a[href^="#"]');
        if (!link || link.classList.contains('js-scroll-top') || link.classList.contains('skip-link')) return;
        var id = link.getAttribute('href');
        if (id.length < 2) return;
        var target = document.querySelector(id);
        if (!target) return;
        e.preventDefault();
        target.scrollIntoView({ behavior: prefersReducedMotion ? 'auto' : 'smooth', block: 'start' });
        history.replaceState(null, '', id);
    });

    /* ---------- Count-up numbers ---------- */
    function animateCount(el) {
        var target = parseInt(el.getAttribute('data-count'), 10);
        if (isNaN(target) || prefersReducedMotion) { el.textContent = target; return; }
        var duration = 1800, start = null;
        function step(ts) {
            if (!start) start = ts;
            var p = Math.min((ts - start) / duration, 1);
            var eased = 1 - Math.pow(1 - p, 3);
            el.textContent = Math.round(target * eased).toLocaleString('en-IN');
            if (p < 1) window.requestAnimationFrame(step);
        }
        el.textContent = '0';
        window.requestAnimationFrame(step);
    }

    /* ---------- Scroll reveal ---------- */
    var revealEls = document.querySelectorAll('[data-reveal]');
    var countEls = document.querySelectorAll('[data-count]');

    if ('IntersectionObserver' in window) {
        var revealObserver = new IntersectionObserver(function (entries, obs) {
            entries.forEach(function (entry) {
                if (entry.isIntersecting) {
                    entry.target.classList.add('is-visible');
                    obs.unobserve(entry.target);
                }
            });
        }, { threshold: 0.12, rootMargin: '0px 0px -40px 0px' });
        revealEls.forEach(function (el) { revealObserver.observe(el); });

        var countObserver = new IntersectionObserver(function (entries, obs) {
            entries.forEach(function (entry) {
                if (entry.isIntersecting) { animateCount(entry.target); obs.unobserve(entry.target); }
            });
        }, { threshold: 0.6 });
        countEls.forEach(function (el) { countObserver.observe(el); });
    } else {
        revealEls.forEach(function (el) { el.classList.add('is-visible'); });
    }

    // Expose a helper so page scripts can reveal dynamically shown elements.
    window.IDS = window.IDS || {};
    window.IDS.revealNow = function (el) { if (el) el.classList.add('is-visible'); };
})();
