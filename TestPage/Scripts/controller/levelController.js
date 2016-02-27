var level = {
    init: function () {
        level.registerEvents();
    },
    registerEvents: function () {
        $('.btn-activeLevel').off('click').on('click', function (e) {
            e.preventDefault();
            var btn = $(this)
            var id = btn.data('id')
            $.ajax({
                url: "../Level/ChangeStatus",
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
level.init();