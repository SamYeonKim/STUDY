// Product Class
class Cup {
    string m_color;         // 색깔
    string m_material;      // 재료
    public Cup() {}

    public void SetColor( string color){
         m_color = color;
    }

    public void SetMaterial( string material){
         m_material = material;
    }
}

//Abstract Builder
abstract class CupBuilder {
    protected Cup m_cup;

    public CupBuilder(){}

    // 컵 반환
    public Cup GetCup(){
         return m_cup; 
    }

    // 컵 생성
    public void CreateNewCup() {
         m_cup = new Cup(); 
    }

    public abstract void BuildColor();
    public abstract void BuildMaterial();
}

//Concrete Builder
class WoodCupBuilder : CupBuilder {
    public override void BuildColor() {
         m_cup.BuildColor("red"); 
    }

    public override void BuildMaterial() {
         m_cup.BuildMaterial("wood"); 
    }
}

//Concrete Builder
class GlassCupBuilder : CupBuilder {
    public override void BuildColor() {
         m_cup.BuildColor("blue"); 
    }

    public override void BuildMaterial() {
         m_cup.BuildMaterial("glass"); 
    }
}

/** "Director" */
class Staff {
    private CupBuilder m_cup_builder;

    // 컵 세팅
    public void SetCupBuilder (CupBuilder cb) {
         m_cup_builder = cb; 
    }

    // 컵 반환
    public Cup GetCup() {
         return m_cup_builder.GetCup(); 
    }

    // 컵 생성
    public void ConstructCup() {
        m_cup_builder.CreateNewCup();
        m_cup_builder.BuildColor();
        m_cup_builder.BuildMaterial();
    }
}

/** A customer ordering a cup. */
class BuilderExample {
    public static void Main(string[] args) {
        Staff staff = new Staff();

        CupBuilder wood_cup_Builder = new WoodCupBuilder();

        staff.SetCupBuilder ( wood_cup_Builder );
        staff.ConstructCup();

        Cup cup = staff.GetCup();
    }
}