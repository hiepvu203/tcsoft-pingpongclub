$(document).ready(function () {
    // Xử lý thay đổi loại Thu/Chi
    $('input[name="Type"]').change(function () {
        var type = $(this).val(); // Lấy giá trị "Thu" (false) hoặc "Chi" (true)
        // Gửi yêu cầu AJAX để lấy lý do
        $.ajax({
            url: '/ExpenseAndIncome/GetReasonsByType', // Đường dẫn đến API
            type: 'GET',
            data: { type: type },
            success: function (reasons) {
                $('#IdReason').empty(); // Xóa các tùy chọn cũ
                $('#IdReason').append('<option value="">Chọn lý do</option>'); // Thêm tùy chọn mặc định
                // Thêm các lý do tương ứng
                $.each(reasons, function (index, reason) {
                    $('#IdReason').append(
                        $('<option></option>')
                            .val(reason.idReason)
                            .text(reason.reasonName)
                    );
                });
            },
            error: function () {
                alert('Không thể tải danh sách lý do. Vui lòng thử lại!');
            }
        });
        // Hiển thị hoặc ẩn checkbox "Thêm hàng loạt"
        var selectedType = $('input[name="Type"]:checked').val();
        if (selectedType == "false") {  // Nếu chọn loại Thu
            $('#bulkAddGroup').show();  // Hiển thị checkbox Thêm hàng loạt
        } else {
            $('#bulkAddGroup').hide();  // Ẩn checkbox khi chọn loại Chi
        }
        // Cập nhật trạng thái hiển thị danh sách thành viên
        toggleMemberList();
    });
    // Lắng nghe sự kiện thay đổi trạng thái checkbox "Thêm hàng loạt"
    $('#isBulkAdd').change(function () {
        toggleMemberList();
    });
    // Hàm xử lý hiển thị danh sách thành viên
    function toggleMemberList() {
        var isBulkAdd = $('#isBulkAdd').is(':checked');
        var typeThu = $('#TypeThu').is(':checked');
        // Trường hợp cả loại Thu và checkbox "Thêm hàng loạt" được chọn
        if (typeThu && isBulkAdd) {
            $('#membersList').hide();
        }
        // Trường hợp checkbox bị bỏ chọn
        else if (typeThu && !isBulkAdd) {
            $('#membersList').show();
        }
        // Các trường hợp khác
        else if (typeThu || isBulkAdd) {
            $('#membersList').show();
        } else {
            $('#membersList').hide();
        }
    }
    // Khởi tạo trạng thái ban đầu
    toggleMemberList();
});