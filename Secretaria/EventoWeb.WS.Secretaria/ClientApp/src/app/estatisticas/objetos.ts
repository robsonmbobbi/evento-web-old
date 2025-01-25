export class DTOEstatisticaTipoInscricao {

  public Criancas: number = 0;

  public Participantes: number = 0;

  public ParticipantesTrabalhadores: number = 0;

  public Trabalhadores: number = 0;

  public CriancasPresentes: number = 0;

  public ParticipantesPresentes: number = 0;

  public ParticipantesTrabalhadoresPresentes: number = 0;

  public TrabalhadoresPresentes: number = 0;
}

export class DTOEstatisticaSexo {

  public Homens: number = 0;
  public Mulheres: number = 0;
  public HomensPresentes: number = 0;
  public MulheresPresentes: number = 0;
}

export class DTOEstatisticaVegetariano {

  public Sao: number = 0;

  public NaoSao: number = 0;

  public SaoPresentes: number = 0;

  public NaoSaoPresentes: number = 0;
}

export class DTOEstatisticaAdocante {

  public Usam: number = 0;
  public NaoUsam: number = 0;
  public UsamPresentes: number = 0;
  public NaoUsamPresentes: number = 0;
}

export class DTOEstatisticaDiabeticos {

  public Sao: number = 0;
  public NaoSao: number = 0;
  public SaoPresentes: number = 0;
  public NaoSaoPresentes: number = 0;
}

export class DTOEstatisticaEvangelizacao {

  public NumeroMeninas: number = 0;
  public NumeroMeninos: number = 0;
  public NumeroCriancas0a3Anos: number = 0;
  public NumeroCriancas4a6Anos: number = 0;
  public NumeroCriancas7a9Anos: number = 0;
  public NumeroCriancas10a12Anos: number = 0;
  public NumeroMeninasPresentes: number = 0;
  public NumeroMeninosPresentes: number = 0;
  public NumeroCriancas0a3AnosPresentes: number = 0;
  public NumeroCriancas4a6AnosPresentes: number = 0;
  public NumeroCriancas7a9AnosPresentes: number = 0;
  public NumeroCriancas10a12AnosPresentes: number = 0;
}

export class DTOEstatisticaCidades {

  public Cidade: String = "";

  public NumeroInscricoes: number = 0;
}

export class DTOEstatisticaGeral {

  public TotalInscricoes: number = 0;

  public TotalInscricoesPresentes: number = 0;

  public TotalInscricoesNaoDormem: number = 0;

  public TotalInscricoesNaoDormemPresentes: number = 0;

  public TiposInscricao!: DTOEstatisticaTipoInscricao;

  public Sexo!: DTOEstatisticaSexo;

  public Vegetarianos!: DTOEstatisticaVegetariano;

  public UsamAdocante!: DTOEstatisticaAdocante;

  public Diabeticos!: DTOEstatisticaDiabeticos;

  public Evangelizacao!: DTOEstatisticaEvangelizacao;

  public CarnesNaoCome: string[] = [];

  public Medicamentos: string[] = [];

  public Alergias: string[] = [];

  public InscritosCidade: DTOEstatisticaCidades[] = [];
}
