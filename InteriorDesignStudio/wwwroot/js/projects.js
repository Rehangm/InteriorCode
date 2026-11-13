/* Projects: category filtering (portfolio) and lightbox gallery (project details) */
(function ($) {
    'use strict';

    /* ================= Portfolio filtering ================= */
    var $grid = $('#portfolioGrid');
    if ($grid.length) {
        var $items = $grid.children('.project-grid__item');
        var $buttons = $('.filter-btn');
        var $empty = $('#portfolioEmpty');
        var $status = $('#filterStatus');
        var current = null;

        function applyFilter(filter, animate) {
            if (filter === current) return;
            current = filter;

            $buttons.each(function () {
                var active = $(this).data('filter') === filter;
                $(this).toggleClass('active', active).attr('aria-pressed', active ? 'true' : 'false');
            });

            var $show = filter === 'all' ? $items : $items.filter('[data-category="' + filter + '"]');
            var $hide = $items.not($show);

            function finish() {
                $hide.addClass('is-hidden').removeClass('is-hiding');
                $show.removeClass('is-hidden');
                // Force reflow so the fade-in transition plays.
                void $grid[0].offsetWidth;
                $show.removeClass('is-hiding').find('[data-reveal]').addBack('[data-reveal]').addClass('is-visible');
                $empty.prop('hidden', $show.length > 0);
                $grid.removeClass('is-loading');
                $status.text($show.length + (filter === 'all' ? ' projects shown' : ' ' + filter.replace('-', ' ') + ' projects shown'));
            }

            if (animate) {
                $grid.addClass('is-loading');
                $items.addClass('is-hiding');
                setTimeout(finish, 350);
            } else {
                finish();
            }

            // Keep the filtered view shareable: /projects?category=kitchen
            if (window.history && window.history.replaceState) {
                var url = filter === 'all' ? window.location.pathname : window.location.pathname + '?category=' + filter;
                window.history.replaceState(null, '', url);
            }
        }

        $buttons.on('click', function () { applyFilter($(this).data('filter'), true); });
        $('[data-filter-reset]').on('click', function () { applyFilter('all', true); });

        applyFilter($grid.data('initial-filter') || 'all', false);
    }

    /* ================= Lightbox ================= */
    var $gallery = $('#projectGallery');
    var $lightbox = $('#lightbox');
    if ($gallery.length && $lightbox.length) {
        var $triggers = $gallery.find('[data-gallery-index]');
        var $img = $lightbox.find('.lightbox__img');
        var $caption = $lightbox.find('.lightbox__caption');
        var index = 0;
        var lastFocus = null;

        function show(i) {
            index = (i + $triggers.length) % $triggers.length;
            var $t = $triggers.eq(index);
            var src = $t.data('full');
            var alt = $t.find('img').attr('alt') || '';

            $lightbox.addClass('is-loading');
            var pre = new Image();
            pre.onload = pre.onerror = function () {
                $img.attr({ src: pre.naturalWidth ? src : '/images/placeholder.svg', alt: alt });
                $caption.text(alt + '  ·  ' + (index + 1) + ' / ' + $triggers.length);
                $lightbox.removeClass('is-loading');
            };
            pre.src = src;

            // Preload neighbours for instant navigation.
            [index + 1, index - 1].forEach(function (n) {
                var $n = $triggers.eq((n + $triggers.length) % $triggers.length);
                (new Image()).src = $n.data('full');
            });
        }

        function open(i) {
            lastFocus = document.activeElement;
            $lightbox.prop('hidden', false);
            requestAnimationFrame(function () { $lightbox.addClass('is-open'); });
            $('body').css('overflow', 'hidden');
            show(i);
            $lightbox.find('[data-lightbox-close]').trigger('focus');
        }

        function close() {
            $lightbox.removeClass('is-open');
            setTimeout(function () { $lightbox.prop('hidden', true); }, 350);
            $('body').css('overflow', '');
            if (lastFocus) lastFocus.focus();
        }

        $triggers.on('click', function () { open(parseInt($(this).data('gallery-index'), 10)); });
        $lightbox.on('click', '[data-lightbox-close]', close);
        $lightbox.on('click', '[data-lightbox-prev]', function () { show(index - 1); });
        $lightbox.on('click', '[data-lightbox-next]', function () { show(index + 1); });
        $lightbox.on('click', function (e) { if (e.target === this) close(); });

        $(document).on('keydown', function (e) {
            if ($lightbox.prop('hidden')) return;
            if (e.key === 'Escape') close();
            else if (e.key === 'ArrowLeft') show(index - 1);
            else if (e.key === 'ArrowRight') show(index + 1);
            else if (e.key === 'Tab') {
                // Simple focus trap
                var $f = $lightbox.find('button:visible');
                var first = $f.first()[0], last = $f.last()[0];
                if (e.shiftKey && document.activeElement === first) { e.preventDefault(); last.focus(); }
                else if (!e.shiftKey && document.activeElement === last) { e.preventDefault(); first.focus(); }
            }
        });

        // Touch swipe
        var startX = null;
        $lightbox.on('touchstart', function (e) { startX = e.originalEvent.touches[0].clientX; });
        $lightbox.on('touchend', function (e) {
            if (startX === null) return;
            var dx = e.originalEvent.changedTouches[0].clientX - startX;
            if (Math.abs(dx) > 50) show(dx < 0 ? index + 1 : index - 1);
            startX = null;
        });
    }
})(jQuery);
