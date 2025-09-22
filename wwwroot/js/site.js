$("#form").on('submit', (event) => {

    event.preventDefault();

    const post = {
        Auteur: {
            Nom: "",
            Prénom: "",
            Id: $("#auteurId").val(),
        },
        Contenu: $("#contenu").val(),
        DatePublication: new Date()
    };
    
    $.ajax({
        type: "POST",
        url: "/Post/Create",
        data: post,
        success: function(data)
        {
            refreshPosts()
        },
        error: function (xhr, status, error) {
            console.error("Erreur AJAX :", error, xhr.responseText);
        }
    });
});

function refreshPosts() {
    $.getJSON("/Post/Get", function(posts) {
        let html = "";
        posts.forEach(post => {
            html += `
                 <div class="card">
                     <h3>${post.auteur.nom} ${post.auteur.prénom}</h3>
                     <p>${post.contenu} </p>
                     <p style="color: var(--dark-grey); font-size: 0.8rem;"> ${post.datePublication} </p>
                 </div>`;
        });
        $("#table-div").html(html);
    });
}