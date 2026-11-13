/* Contact page: validation UX, character counter, loading state, success focus */
(function ($) {
    'use strict';

    var $form = $('#contactForm');

    // Focus the success message after a successful submission (accessibility).
    var success = document.getElementById('formSuccess');
    if (success) {
        var card = document.getElementById('enquiry-form');
        if (card) card.classList.add('is-visible');
        setTimeout(function () {
            success.scrollIntoView({ behavior: 'smooth', block: 'center' });
            success.focus({ preventScroll: true });
        }, 400);
    }

    if (!$form.length) return;

    // Message character counter
    var $message = $('#Input_Message');
    var $count = $('#messageCount');
    function updateCount() {
        var len = ($message.val() || '').length;
        $count.text(len + ' / 2000');
    }
    $message.on('input', updateCount);
    updateCount();

    // Only allow phone-friendly characters.
    $('#Input_Phone').on('input', function () {
        var cleaned = this.value.replace(/[^0-9+\-\s()]/g, '');
        if (cleaned !== this.value) this.value = cleaned;
    });

    // Validate fields as soon as the user leaves them.
    if ($.validator) {
        $form.find('input, select, textarea').on('blur change', function () {
            if (this.name && this.name.indexOf('Website') === -1 && $(this).val()) {
                $(this).valid();
            }
        });
        $form.find('.form-control, .form-select').on('keyup change', function () {
            var $f = $(this);
            if ($f.hasClass('input-validation-error') || !$f.val()) { $f.removeClass('is-valid-field'); return; }
            $f.toggleClass('is-valid-field', $f.hasClass('valid'));
        });
    }

    // Loading state + scroll to first error
    $form.on('submit', function () {
        var isValid = $.validator ? $form.valid() : this.checkValidity();
        if (!isValid) {
            var $firstError = $form.find('.input-validation-error').first();
            if ($firstError.length) {
                $('html, body').animate({ scrollTop: $firstError.offset().top - 140 }, 400);
                $firstError.trigger('focus');
            }
            return false;
        }
        $('#submitBtn').addClass('is-loading').attr('aria-busy', 'true');
        return true;
    });
})(jQuery);
