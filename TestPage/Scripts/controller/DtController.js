var dt = {
    init: function () {
        dt.registerEvents();
    },
    registerEvents: function () {
        $('.btn-activeDt').off('click').on('click', function (e) {
            e.preventDefault();
            var btn = $(this)
            var id = btn.data('id')
            $.ajax({
                url: "../Dt/ChangeStatus",
                data: { id: id },
                dataType: "json",
                type: "POST",
                success: function (response) {
                    if (response.status == true) {
                        btn.text('Khóa');
                    }
                    else {
                        btn.text('Kích hoạt');
                    }
                }
            });
        });
    }
}
dt.init();