var user = {
    init: function () {
        user.registerEvents();
    },
    registerEvents: function () {
        $('.btn-active').off('click').on('click', function (e) {
            e.preventDefault();
            var btn=$(this)
            var id = btn.data('id')
            $.ajax({
                url: "../User/ChangeStatus",
                data: { id: id },
                dataType: "json",
                type:  "POST",
                success: function (response) {
                    if(response.status==true)
                    {
                        btn.text('Khóa');
                    }
                    else
                    {
                        btn.text('Kích hoạt');
                    }
                }
                });
        });


        $('.btn-thi').off('click').on('click', function (e) {
            e.preventDefault();
            var btn = $(this)
            var id = btn.data('id')
            $.ajax({
                url: "../User/ChangeStatusThi",
                data: { id: id },
                dataType: "json",
                type: "POST",
                success: function (response) {
                    if (response.status == true) {
                        btn.text('Chưa thi');
                    }
                    else {
                        btn.text('Đã thi');
                    }
                }
            });
        });
    }
}
user.init();


var user1 = {
    init: function () {
        user1.registerEvents();
    },
    registerEvents: function () {
    
    }
}
user1.init();