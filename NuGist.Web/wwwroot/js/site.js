// Write your Javascript code.
$('.table tr').each(function (index, element) {
    var href = $(this).data('href');
    if (href)
        $(this).children('td:not(:last)')
            .click(function (e) {
                e.preventDefault();
                window.document.location = href;
            });
});