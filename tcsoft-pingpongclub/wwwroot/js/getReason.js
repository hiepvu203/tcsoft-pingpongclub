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
                    console.log('Reason RecurringFee:', reason.recurringFee);
                    $('#IdReason').append(
                        $('<option></option>')
                            .val(reason.idReason)
                            .text(reason.reasonName)
                            .data('recurringFee', reason.recurringFee) // Lưu recurringFee vào data attribute
                    );
                });
            },
            error: function () {
                alert('Không thể tải danh sách lý do. Vui lòng thử lại!');
            }
        });
            
        // Cập nhật trạng thái bật/tắt danh sách thành viên
        toggleMemberList();
    });

    // Lắng nghe sự kiện thay đổi lựa chọn lý do
    $('#IdReason').change(function () {
        toggleMemberList();
    });

    // Hàm xử lý bật/tắt danh sách thành viên
    function toggleMemberList() {
        var selectedReason = $('#IdReason').find('option:selected'); // Lấy lựa chọn lý do
        var recurringFee = selectedReason.data('recurringFee'); // Lấy giá trị recurringFee từ data attribute

        console.log(recurringFee);

        // Disable danh sách thành viên nếu recurringFee = true, ngược lại
        if (recurringFee === true) {
            $('.showList select').prop('disabled', true); 
        } else {
            $('.showList select').prop('disabled', false);
        }
    }
    toggleMemberList();
});
