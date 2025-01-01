// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web asset
    // Thêm sự kiện click cho các phần tử có class .parent
    document.querySelectorAll('.parent').forEach(function(parent) {
        parent.addEventListener('click', function () {
            // Lấy các phần tử .child và .child1 trong cùng một dòng
            const childRows = parent.nextElementSibling;
            const child1Rows = parent.nextElementSibling.nextElementSibling;

            // Kiểm tra xem các phần tử có đang hiển thị không
            if (childRows.style.display === 'table-row' || child1Rows.style.display === 'table-row') {
                // Nếu hiển thị, ẩn chúng
                childRows.style.display = 'none';
                child1Rows.style.display = 'none';
            } else {
                childRows.style.display = 'table-row';
                child1Rows.style.display = 'table-row';
            }
        });
    });


