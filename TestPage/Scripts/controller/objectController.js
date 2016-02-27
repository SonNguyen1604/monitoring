var object = {
    init: function () {
        object.registerEvents();
    },
    registerEvents: function () {
        $('.btn-activeObject').off('click').on('click', function (e) {
            e.preventDefault();
            var btn = $(this)
            var id = btn.data('id')
            $.ajax({
                url: "../Object/ChangeStatus",
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
object.init();