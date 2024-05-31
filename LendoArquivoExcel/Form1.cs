using Microsoft.VisualBasic.FileIO;

namespace LendoArquivoExcel
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnProcurar_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                // Configuração do OpenFileDialog
                openFileDialog.Filter = "Arquivos CSV (*.csv)|*.csv|Todos os arquivos (*.*)|*.*";
                openFileDialog.Title = "Selecione o arquivo CSV";

                if (openFileDialog.ShowDialog() != DialogResult.OK)
                    return;

                string caminhoArquivo = openFileDialog.FileName;

                try
                {
                    using (TextFieldParser parser = new TextFieldParser(caminhoArquivo))
                    {
                        parser.TextFieldType = FieldType.Delimited;
                        parser.SetDelimiters(";");

                        // Ignora a linha de cabeçalho se houver
                        if (!parser.EndOfData)
                        {
                            string[] cabecalho = parser.ReadFields();
                            // Encontre o índice das colunas desejadas
                            int IndexId = Array.IndexOf(cabecalho, "Id");
                            int IndexCategoria = Array.IndexOf(cabecalho, "DescricaoCategoria");
                            int IndexCreateAt = Array.IndexOf(cabecalho, "CreateAt");
                            int IndexUpdateAt = Array.IndexOf(cabecalho, "UpdateAt");
                            int IndexHabilitado = Array.IndexOf(cabecalho, "Habilitado");

                            List<CategoriaProdutoDto> categorias = new List<CategoriaProdutoDto>();
                            // Itera sobre as linhas do arquivo
                            while (!parser.EndOfData)
                            {
                                string[] campo = parser.ReadFields();

                                var categoria = new CategoriaProdutoDto
                                {
                                    Id = Guid.Parse(campo[IndexId]),
                                    CreateAt = Convert.ToDateTime(campo[IndexCreateAt]),
                                    DescricaoCategoria = campo[IndexCategoria],
                                    Habilitado = Convert.ToInt32(campo[IndexHabilitado]) == 1 ? true : false,
                                    UpdateAt = campo[IndexUpdateAt] == string.Empty ? null : Convert.ToDateTime(campo[IndexUpdateAt])
                                };

                                categorias.Add(categoria);
                            }

                            dt.DataSource = categorias;
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Erro.Detalhes: {ex.Message}");
                }
            }
        }

        public class CategoriaProdutoDto
        {
            public Guid Id { get; set; }
            public DateTime CreateAt { get; set; }
            public DateTime? UpdateAt { get; set; }
            public bool Habilitado { get; set; }
            public string? DescricaoCategoria { get; set; }
        }
    }
}
