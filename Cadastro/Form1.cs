using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace Cadastro
{
    public partial class Form1 : Form
    {
        MySqlConnection conexao;
        MySqlCommand comando;
        MySqlDataAdapter da;
        string query;
        bool isEdit = false;
        string id_cliente;


        public Form1()
        {
            InitializeComponent();
            preenche();
        }

        private void btnSalvar_Click(object sender, EventArgs e)
        {
            if (isEdit)
            {
                editar();
            }
            else
            {
                salvar();
            }
        }


        private void limpar()
        {
            id_cliente = "";
            txtNome.Text = "";
            txtDdd.Text = "";
            txtTelefone.Text = "";
            conexao = null;
            comando = null;
            isEdit = false;
            da = null;

        }

        private void preenche()
        {
            try
            {
                conexao = new MySqlConnection("Server=localhost;Database=cadastro;Uid=root;Pwd=mysql;");
                query = "select t.id as id_cliente, t.ddd as DDD, t.telefone as Telefone, c.nome as Nome from telefone t, cliente c where c.id = t.cliente_id;";

                da = new MySqlDataAdapter(query, conexao);
                DataTable t = new DataTable();
                da.Fill(t);

                dGV.DataSource = t;


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {

            }
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            isEdit = true;

            id_cliente = dGV.SelectedRows[0].Cells["ID"].Value.ToString();
            txtNome.Text = dGV.SelectedRows[0].Cells["Nome"].Value.ToString();
            txtDdd.Text = dGV.SelectedRows[0].Cells["DDD"].Value.ToString();
            txtTelefone.Text = dGV.SelectedRows[0].Cells["Telefone"].Value.ToString();
        }

        private void btnExcluir_Click(object sender, EventArgs e)
        {
            try
            {
                id_cliente = dGV.SelectedRows[0].Cells["ID"].Value.ToString();


                conexao = new MySqlConnection("Server=localhost;Database=cadastro;Uid=root;Pwd=mysql;");
                query = "CALL DELETACLIENTE(@ID)";

                comando = new MySqlCommand(query, conexao);
                comando.Parameters.AddWithValue("@ID", id_cliente);

                conexao.Open();

                comando.ExecuteNonQuery();
                preenche();
                MessageBox.Show("Excluído!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                conexao.Close();
                limpar();
            }
        }

        private void salvar()
        {
            try
            {
                conexao = new MySqlConnection("Server=localhost;Database=cadastro;Uid=root;Pwd=mysql;");
                query = "CALL INSERECLIENTE(@NOME, @DDD, @TELEFONE)";

                comando = new MySqlCommand(query, conexao);
                comando.Parameters.AddWithValue("@NOME", txtNome.Text);
                comando.Parameters.AddWithValue("@DDD", txtDdd.Text);
                comando.Parameters.AddWithValue("@TELEFONE", txtTelefone.Text);

                conexao.Open();

                comando.ExecuteNonQuery();
                preenche();
                MessageBox.Show("Cadastrado!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                conexao.Close();
                limpar();
            }
        }

        private void editar()
        {
            try
            {
                conexao = new MySqlConnection("Server=localhost;Database=cadastro;Uid=root;Pwd=mysql;");
                query = "CALL ALTERACLIENTE(@NOME, @DDD, @TELEFONE, @id)";

                comando = new MySqlCommand(query, conexao);
                comando.Parameters.AddWithValue("@NOME", txtNome.Text);
                comando.Parameters.AddWithValue("@DDD", txtDdd.Text);
                comando.Parameters.AddWithValue("@TELEFONE", txtTelefone.Text);
                comando.Parameters.AddWithValue("@ID", id_cliente);

                conexao.Open();

                comando.ExecuteNonQuery();
                preenche();
                MessageBox.Show("Alterado com sucesso!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                conexao.Close();
                limpar();
            }
        }

        private void btnLimpar_Click(object sender, EventArgs e)
        {
            limpar();
        }
    }
}
