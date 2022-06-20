function Excluir(id, controller) {
    Swal.fire({
        icon: 'question',
        title: "Prosseguir com a exclusão?",
        showDenyButton: true,
        confirmButtonText: 'Sim',
        denyButtonText: 'Não'
    }).then(confirmacao => {
        if (confirmacao.isConfirmed) {
            const url = `/${controller}/Excluir`;

            $.ajax({
                url: url,
                method: 'POST',
                data: { id: id },
                success: function () {
                    $(`.${id}`).remove();
                    Swal.fire('Registro excluido com sucesso', '', 'secess');
                }
            })
        }
    })
}