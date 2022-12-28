// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
function openErrorModal(strMessage) {
    var myDiv = document.getElementById("MyModalErrorAlertBody");
    myDiv.innerHTML = strMessage;
    $('#myModalError').modal('show');
}

function openSuccessModal(strMessage) {
    var myDiv = document.getElementById("MyModalSuccessAlertBody");
    myDiv.innerHTML = strMessage;
    $('#myModalSuccess').modal('show');
}

$(document).ready(function () {
    $('.remove').click(function (e) {
        e.preventDefault();
        var productId = $(this).data('productid');
        var url = $(this).attr('href');
        $.ajax({
            url: url,
            type: 'POST',
            success: function (data) {
                if (data.success) {
                    if (data.count == 0) {
                        $('.shopping-item').html("");
                        $('.shopping-item').html('<div class="dropdown-cart-header"><p>Không có sản phẩm trong giỏ hàng</p></div > ');
                    } else {
                        $('#remove-' + productId).parent().parent().parent().remove();
                        $('#cart-count').text(data.count);
                        $('#cart-total').text(data.total);
                    }
                }
                else {
                    openErrorModal(data.message);
                    return false;
                }
            }
        });
    });
});