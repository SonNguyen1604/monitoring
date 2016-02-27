var exam = {
    init: function () {
        exam.registerEvents();
    },
    registerEvents: function () {
        $('.btn-activeExam').off('click').on('click', function (e) {
            e.preventDefault();
            var btn = $(this)
            var id = btn.data('id')
            $.ajax({
                url: "../Exam/ChangeStatus",
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
exam.init();