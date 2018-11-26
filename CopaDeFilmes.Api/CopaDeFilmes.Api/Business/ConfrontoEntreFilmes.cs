using System.Linq;
using CopaDeFilmes.Api.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace CopaDeFilmes.Api.Business
{
    public class ConfrontoEntreFilmes
    {
        public IList<Filmes> ListaDeFilmesOrdenadas { get; }
        public IList<Filmes> ListaVersus { get; }
        public ConfrontoEntreFilmes()
        {
            ListaDeFilmesOrdenadas = new List<Filmes>();
            ListaVersus = new List<Filmes>();
        }
        public IList<Filmes> ObterVencedor(IList<Filmes> listaDeFilmes)
        {
            int quantidadeVencedores = (listaDeFilmes.Count / 2);

            OrdenarListaDeFilmesPorNome(listaDeFilmes);
            
            for(int i = 0; i < 3; i++)
            {
                ListaVersus.Clear();

                for(int j = 0; j < quantidadeVencedores; j++)
                {
                    Filmes filme = listaDeFilmes.Take(1).FirstOrDefault();
                    Filmes filme2 = listaDeFilmes[listaDeFilmes.Count - 1];
                    if(quantidadeVencedores == 1)
                    {
                        ListaVersus.Add(filme);
                        ListaVersus.Add(filme2);
                    }
                    else
                    {
                        DisputaEntre(filme, filme2);
                    }

                    listaDeFilmes.Remove(filme);
                    listaDeFilmes.Remove(filme2);
                }

                if(quantidadeVencedores == 1)
                    if(ListaVersus.Take(1).FirstOrDefault().nota == ListaVersus[ListaVersus.Count - 1].nota)
                        ListaVersus.OrderBy(w => w.titulo).ToList().ForEach(item => listaDeFilmes.Add(item));
                    else
                        ListaVersus.ToList().ForEach(item => listaDeFilmes.Add(item));
                else
                {
                    listaDeFilmes.Clear();
                    ListaVersus.ToList().ForEach(item => listaDeFilmes.Add(item));
                    quantidadeVencedores = (listaDeFilmes.Count / 2);
                }
            }

            return listaDeFilmes;
        }
        private void OrdenarListaDeFilmesPorNome(IList<Filmes> listaDeFilmes)
        {
            ListaDeFilmesOrdenadas.Clear();
            listaDeFilmes.OrderBy(w => w.titulo).ToList().ForEach(item => ListaDeFilmesOrdenadas.Add(item));

            listaDeFilmes.Clear();
            ListaDeFilmesOrdenadas.ToList().ForEach(item => listaDeFilmes.Add(item));
        }
        private void DisputaEntre(Filmes filme, Filmes filme2)
        {
            if(filme.nota > filme2.nota)
            {
               ListaVersus.Add(filme);
            }
            else if(filme2.nota > filme.nota)
            {
               ListaVersus.Add(filme2);
            }
            else
            {
                string[] filmes = { filme.titulo, filme2.titulo };
                IEnumerable<string> query = from f in filmes
                                            orderby f.Length, f.Substring(0, 1)
                                            select f;

                foreach(string str in query)
                {
                    if(str == filme.titulo)
                    {
                      ListaVersus.Add(filme);
                    }
                    else
                    {
                      ListaVersus.Add(filme2);
                    }
                }

            }
        }
    }
}
