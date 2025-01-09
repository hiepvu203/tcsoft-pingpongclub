// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web asset
    document.querySelectorAll('.parent').forEach(function(parent) {
        parent.addEventListener('click', function () {
            const childRows = parent.nextElementSibling;
            const child1Rows = parent.nextElementSibling.nextElementSibling;
            if (childRows.style.display === 'table-row' || child1Rows.style.display === 'table-row') {
                childRows.style.display = 'none';
                child1Rows.style.display = 'none';
            } else {
                childRows.style.display = 'table-row';
                child1Rows.style.display = 'table-row';
            }
        });
    });


