

import com.java4less.rchart.*;
import com.java4less.rchart.gc.ChartFont;
import com.java4less.rchart.gc.ChartColor;
import com.java4less.rchart.gc.ChartImage;
import com.java4less.rchart.gc.GraphicsProvider;

/**
 * 
 * Set of examples for RChart using the Java API
 */
public class ChartExamples {
	
		private static String[] lbls={"June","July","Aug.","Sept.","Oct.","Nov.","Dec."};
				

	/**
	 * line 3D example
	 * @return
	 */
		public static Chart Example9() {

		   // data
		  double[] d1={1,1,3,3.5,5,4,2};
		  double[] d2={2,2,5,6,5.4,3.5,3.1};

		  // series
		  LineDataSerie data1= new LineDataSerie(d1,new LineStyle(1,GraphicsProvider.getColor(ChartColor.BLUE),LineStyle.LINE_NORMAL));
		  data1.drawPoint=false;
		  data1.valueFont=GraphicsProvider.getFont("Arial",ChartFont.PLAIN,11);		  
		  LineDataSerie data2= new LineDataSerie(d2,new LineStyle(1,GraphicsProvider.getColor(ChartColor.GREEN),LineStyle.LINE_NORMAL));
		  data2.drawPoint=false;
		  data2.valueFont=GraphicsProvider.getFont("Arial",ChartFont.PLAIN,11);
		  data2.fillStyle=new FillStyle(GraphicsProvider.getColor(ChartColor.GREEN));

		  // legend
		  Legend l=new Legend();
		  l.addItem("Products",new FillStyle(GraphicsProvider.getColor(ChartColor.BLUE)));
		  l.addItem("Services",new FillStyle(GraphicsProvider.getColor(ChartColor.GREEN)));

		  // create title
		  Title title=new Title("Sales (thousands $)");
		  // create axis
		  Axis  XAxis=new Axis(Axis.HORIZONTAL,new Scale());
		  Axis  YAxis=new Axis(Axis.VERTICAL,new Scale());
		  XAxis.tickAtBase=true; // draw also first tick
		  XAxis.scale.min=0;
		  YAxis.scale.min=0;
		  YAxis.scaleTickInterval=1;
		  XAxis.scaleTickInterval=1;
		  YAxis.bigTickInterval=2;
		  YAxis.gridStyle=new LineStyle(1,GraphicsProvider.getColor(ChartColor.GRAY),LineStyle.LINE_NORMAL);
		  String[] lbls={"June","July","Aug.","Sept.","Oct.","Nov.","Dec."};
		  XAxis.tickLabels=lbls;


		  HAxisLabel XLabel= new HAxisLabel("",GraphicsProvider.getColor(ChartColor.BLUE),GraphicsProvider.getFont("Arial",ChartFont.PLAIN,14));
		  VAxisLabel YLabel= new VAxisLabel("Brutto",GraphicsProvider.getColor(ChartColor.BLACK),GraphicsProvider.getFont("Arial",ChartFont.PLAIN,14));

		  // plotter
		  LinePlotter3D plot=new LinePlotter3D();

		  // create report
		  Chart chart=new Chart(title,plot,XAxis,YAxis);
		  plot.border=new LineStyle(1,GraphicsProvider.getColor(ChartColor.BLACK),LineStyle.LINE_NORMAL);
		  plot.back=new FillStyle(GraphicsProvider.getColor(ChartColor.WHITE));
		  chart.back=new FillStyle(GraphicsProvider.getColor(ChartColor.YELLOW));
		  chart.back.colorFrom=GraphicsProvider.getColor(ChartColor.YELLOW);
		  chart.back.colorUntil=GraphicsProvider.getColor(ChartColor.WHITE);
		  chart.back.gradientType=FillStyle.GRADIENT_HORIZONTAL;
		  chart.XLabel=XLabel;
		  chart.YLabel=YLabel;
		  chart.legend=l;
		  chart.addSerie(data2);
		  chart.addSerie(data1);

		  return chart;


		}

	/**
	 * bar 3D example
	 * @return
	 */
	public static Chart Example8() {

		 // data
		  double[] d1={1,2,1,4,5,4,2};
		  BarDataSerie data1= new BarDataSerie(d1,new FillStyle(GraphicsProvider.getColor(ChartColor.ORANGE)));		
		  data1.borderType=BarDataSerie.BORDER_RAISED;
		  data1.valueFont=GraphicsProvider.getFont("Arial",ChartFont.PLAIN,10);

		  double[] d2={-1,3,4,4.2,6.4,4.5,6.1};
		  BarDataSerie data2= new BarDataSerie(d2,new FillStyle(GraphicsProvider.getColor(ChartColor.GREEN)));
		  data2.valueFont=GraphicsProvider.getFont("Arial",ChartFont.PLAIN,10);
		  data2.borderType=BarDataSerie.BORDER_RAISED;
		  data2.negativeStyle=new FillStyle(GraphicsProvider.getColor(ChartColor.RED));


		  Legend l=new Legend();
		  l.border=new LineStyle(1,GraphicsProvider.getColor(ChartColor.BLACK),LineStyle.LINE_NORMAL);
		  l.addItem("Company A",new FillStyle(GraphicsProvider.getColor(ChartColor.ORANGE)));
		  l.addItem("Company B",new FillStyle(GraphicsProvider.getColor(ChartColor.GREEN)));
		  l.background=new FillStyle(GraphicsProvider.getColor(ChartColor.LIGHTGRAY));

		  // create title
		  Title title=new Title("Benefits companies A & B");
		  // create axis
		  Axis  XAxis=new Axis(Axis.HORIZONTAL,new Scale());
		  Axis  YAxis=new Axis(Axis.VERTICAL,new Scale());
		  Axis  Y2Axis=new Axis(Axis.VERTICAL,new Scale());

		  XAxis.tickAtBase=true; // draw also first tick
		  XAxis.scale.min=0;
		  XAxis.scale.max=7;
		  YAxis.scale.min=-2;
		  YAxis.scale.max=7;
		  YAxis.IntegerScale=true;

		  Y2Axis.scale.min=-2;
		  Y2Axis.scale.max=7;
		  Y2Axis.IntegerScale=true;
		  Y2Axis.scaleTickInterval=1;

		  YAxis.scaleTickInterval=1;
		  XAxis.scaleTickInterval=1;
		  XAxis.gridStyle=new LineStyle(1,GraphicsProvider.getColor(ChartColor.LIGHTGRAY),LineStyle.LINE_DOTS);
		  YAxis.gridStyle=new LineStyle(1,GraphicsProvider.getColor(ChartColor.LIGHTGRAY),LineStyle.LINE_DOTS);
		  String[] lbls={"1992","1993","1994","1995","1996","1997","1998","1999"};
		  XAxis.tickLabels=lbls;

		  HAxisLabel XLabel= new HAxisLabel("Year",GraphicsProvider.getColor(ChartColor.BLACK),GraphicsProvider.getFont("Arial",ChartFont.PLAIN,12));
		  VAxisLabel YLabel= new VAxisLabel("million $",GraphicsProvider.getColor(ChartColor.BLACK),GraphicsProvider.getFont("Arial",ChartFont.PLAIN,12));
			YLabel.vertical=true;
		
		  // plotter
		  BarPlotter3D plot=new BarPlotter3D();
		  // create report
		  Chart chart=new Chart(title,plot,XAxis,YAxis);
		  plot.interBarSpace=1;
		  plot.back=new FillStyle(GraphicsProvider.getColor(ChartColor.WHITE));
		  plot.depth=20;
		  plot.fullDepth=true;
		  //chart.setY2Scale(Y2Axis);
		  chart.XLabel=XLabel;
		  chart.YLabel=YLabel;
		  chart.legend=l;
		  chart.addSerie(data2);
		  chart.addSerie(data1);
		  chart.legendMargin=0.25;
		  chart.back=new FillStyle(GraphicsProvider.getColor(ChartColor.YELLOW));

  		  chart.backImage=GraphicsProvider.getImage("back13.gif");

		 return chart;


		}

		/**
		 * bubble chart example
		 * @return
		 */
		public static Chart Example13() {

		  // first

		   // data
		  double[] d1={2,1 ,2 ,3 ,4 ,5 ,4 ,3};
		  double[] dmax1={0.5,0 ,0.2 ,0.1 ,0.5 ,0,1 ,0};
		  MaxMinDataSerie data1= new MaxMinDataSerie(d1,null);
		  data1.bubbleChart=true;
		  data1.fillBubble=true;
		  data1.drawPoint=true;
		  data1.pointColor=GraphicsProvider.getColor(ChartColor.GREEN);
		  data1.setMaxMinValues(dmax1,null);

		  double[] d2={2,1 ,2 ,3 ,4 ,5 ,4 ,3};
		  LineDataSerie data2= new LineDataSerie(d2,new LineStyle(1,GraphicsProvider.getColor(ChartColor.WHITE),LineStyle.LINE_DOTS));
		  data2.valueFont=GraphicsProvider.getFont("Arial",ChartFont.PLAIN,10);
		  data2.valueColor=GraphicsProvider.getColor(ChartColor.YELLOW);
		  data2.valueFont=GraphicsProvider.getFont("Arial",ChartFont.PLAIN,10);

		  // second

		  double[] d3={1,2 ,3 ,2 ,3 ,4 ,1 ,2};
		  double[] dmax3={0,0.2 ,0.2 ,0 ,0.2 ,0,0.3 ,0};
		  MaxMinDataSerie data3= new MaxMinDataSerie(d3,null);
		  data3.bubbleChart=true;
		  data3.fillBubble=false;
		  data3.drawPoint=true;
		  data3.pointColor=GraphicsProvider.getColor(ChartColor.WHITE);
		  data3.setMaxMinValues(dmax3,null);

		  double[] d4={1,2 ,3 ,2 ,3 ,4 ,1 ,2};
		  LineDataSerie data4= new LineDataSerie(d4,new LineStyle(1,GraphicsProvider.getColor(ChartColor.WHITE),LineStyle.LINE_DOTS));
		  data4.valueFont=GraphicsProvider.getFont("Arial",ChartFont.PLAIN,10);
		  data4.valueColor=GraphicsProvider.getColor(ChartColor.YELLOW);
		  data4.valueFont=GraphicsProvider.getFont("Arial",ChartFont.PLAIN,10);

		  // create title
		  Title title=new Title("Price");
		  title.color=GraphicsProvider.getColor(ChartColor.WHITE);

		  // create axis
		  Axis  XAxis=new Axis(Axis.HORIZONTAL,new Scale());
		  Axis  YAxis=new Axis(Axis.VERTICAL,new Scale());
		  XAxis.tickAtBase=true; // draw also first tick
		  XAxis.scale.min=-1;
		  XAxis.ceroAxis=Axis.CEROAXIS_NO;
		  YAxis.ceroAxis=Axis.CEROAXIS_NO;
		  YAxis.scale.min=0;
		  YAxis.scale.max=6;
		  YAxis.DescColor=GraphicsProvider.getColor(ChartColor.WHITE);
		  YAxis.scaleTickInterval=1;
		  XAxis.scaleTickInterval=1;
		  XAxis.bigTickInterval=1;
		  XAxis.DescColor=GraphicsProvider.getColor(ChartColor.WHITE);
		  String[] lbls={" ","8 Jan.","9 Jan.","10 Jan.","11 Jan.","12 Jan.","13 Jan.","14 Jan.","15 Jan."};
		  XAxis.tickLabels=lbls;
		  XAxis.style =new LineStyle(1,GraphicsProvider.getColor(ChartColor.WHITE),LineStyle.LINE_NORMAL);
		  YAxis.style =new LineStyle(1,GraphicsProvider.getColor(ChartColor.WHITE),LineStyle.LINE_NORMAL);

		  HAxisLabel XLabel= new HAxisLabel("Week",GraphicsProvider.getColor(ChartColor.WHITE),GraphicsProvider.getFont("Arial",ChartFont.PLAIN,12));
		  VAxisLabel YLabel= new VAxisLabel("Value",GraphicsProvider.getColor(ChartColor.WHITE),GraphicsProvider.getFont("Arial",ChartFont.PLAIN,12));

		  // plotter
		  LinePlotter plot=new LinePlotter();

		  // create report
		  Chart chart=new Chart(title,plot,XAxis,YAxis);
		  chart.XLabel=XLabel;
		  chart.YLabel=YLabel;
		  chart.addSerie(data1);
		  chart.addSerie(data2);

		  chart.addSerie(data4);
		  chart.addSerie(data3);
		  chart.back=new FillStyle(GraphicsProvider.getColor(ChartColor.BLUE));


		  return chart;

		}


	/**
	 * Pie chart example
	 * @return
	 */
		public static  Chart Example3() {

		  // data
		  //double[] d1={64,95,11,70};
		  double[] d1={9918,22652,511,36811};


			  // style of the pie
		  FillStyle[] s1={new FillStyle(GraphicsProvider.getColor(ChartColor.CYAN)),new FillStyle(GraphicsProvider.getColor(ChartColor.BLUE)),new FillStyle(GraphicsProvider.getColor(ChartColor.GREEN)),new FillStyle(GraphicsProvider.getColor(ChartColor.YELLOW))};
		  PieDataSerie data1= new PieDataSerie(d1,s1);
		  data1.textDistanceToCenter=1.2;
		  data1.valueFont=GraphicsProvider.getFont("Arial",ChartFont.BOLD,13);
		  data1.textDistanceToCenter=1.1;

			  // legend
		  Legend l=new Legend();
		  l.background=new FillStyle(GraphicsProvider.getColor(ChartColor.LIGHTGRAY));
		  l.border=new LineStyle(1,GraphicsProvider.getColor(ChartColor.BLACK),LineStyle.LINE_NORMAL);
		  l.addItem("Software",new FillStyle(GraphicsProvider.getColor(ChartColor.BLUE)));
		  l.addItem("Hardware",new FillStyle(GraphicsProvider.getColor(ChartColor.GREEN)));
		  l.addItem("Services",new FillStyle(GraphicsProvider.getColor(ChartColor.YELLOW)));
		  l.addItem("Other 1",new FillStyle(GraphicsProvider.getColor(ChartColor.CYAN)));


		  // create title
		  Title title=new Title("Sales 2005");

		  // plotter
		  PiePlotter plot=new PiePlotter();
		  plot.radiusModifier=0.6;

		  plot.labelLine=new LineStyle(1,GraphicsProvider.getColor(ChartColor.BLACK),LineStyle.LINE_NORMAL);

		 // create chart
		  Chart chart=new Chart(title,plot,null,null);
		  // chart background
		  chart.back=new FillStyle(GraphicsProvider.getColor(ChartColor.DARKGRAY));
		  chart.back.gradientType=FillStyle.GRADIENT_VERTICAL;          
		  // add legend
		  chart.legend=l;
		  // add data
		  chart.addSerie(data1);


		  chart.backImage=GraphicsProvider.getImage("back16.gif");;

		  return chart;

		}


	/**
	 * Pie chart 3D example
	 * @return
	 */
	public static  Chart Example4() {
	  // data
		  double[] d1={34,34,34,34,34};
		  boolean[] b1={true,false,true,true};
		  String[] labels={"Software","Hardware","Services","Other"};
		  // style of the pie
		  FillStyle[] s1={new FillStyle(GraphicsProvider.getColor(ChartColor.CYAN)),new FillStyle(GraphicsProvider.getColor(ChartColor.BLUE)),new FillStyle(GraphicsProvider.getColor(ChartColor.GREEN)),new FillStyle(GraphicsProvider.getColor(ChartColor.YELLOW)),new FillStyle(GraphicsProvider.getColor(ChartColor.RED))};
		  PieDataSerie data1= new PieDataSerie(d1,s1,b1,labels);
		  data1.valueFont=GraphicsProvider.getFont("Arial",ChartFont.PLAIN,14);

		  // legend
		  Legend l=new Legend();
		  l.background=new FillStyle(GraphicsProvider.getColor(ChartColor.LIGHTGRAY));
		  l.border=new LineStyle(1,GraphicsProvider.getColor(ChartColor.BLACK),LineStyle.LINE_NORMAL);
		  l.addItem(labels[0],new FillStyle(GraphicsProvider.getColor(ChartColor.BLUE)));
		  l.addItem(labels[1],new FillStyle(GraphicsProvider.getColor(ChartColor.GREEN)));
		  l.addItem(labels[2],new FillStyle(GraphicsProvider.getColor(ChartColor.YELLOW)));
		  l.addItem(labels[3],new FillStyle(GraphicsProvider.getColor(ChartColor.CYAN)));
			l.addItem(labels[3],new FillStyle(GraphicsProvider.getColor(ChartColor.GREEN)));

		  // create title
		  Title title=new Title("Sales 2005");

		  // plotter
		  PiePlotter plot=new PiePlotter();
		  // 3D effect
		  plot.effect3D=true;
		  plot.border=new LineStyle(1,GraphicsProvider.getColor(ChartColor.BLACK),LineStyle.LINE_NORMAL);

		  data1.textDistanceToCenter=0.3;
		  plot.labelFormat="#PERCENTAGE#";

		  // create chart
		  Chart chart=new Chart(title,plot,null,null);
		  // chart background
		  chart.back=new FillStyle(GraphicsProvider.getColor(ChartColor.DARKGRAY));
		  chart.back.gradientType=FillStyle.GRADIENT_VERTICAL;

		  // add legend
		  chart.legend=l;
		  // legend position and layout
		  chart.layout=Chart.LAYOUT_LEGEND_BOTTOM;
		  chart.bottomMargin=0;
		  chart.topMargin=0.2; // 20%
		  l.verticalLayout=false;
		  // add data
		  chart.addSerie(data1);

		  chart.backImage=GraphicsProvider.getImage("back16.gif");

		  return chart;

		}

	/**
	 * bar chart example
	 * @return
	 */
		public static  Chart Example2() {

		  // data
		  double[] d1={1,2,3,4,5,4,2};
		  BarDataSerie data1= new BarDataSerie(d1,new FillStyle(GraphicsProvider.getColor(ChartColor.CYAN)));
		  data1.border=new LineStyle(1,GraphicsProvider.getColor(ChartColor.BLACK),LineStyle.LINE_NORMAL);
		  //data1.borderType=BarDataSerie.BORDER_RAISED;
		  data1.border=new LineStyle(1,GraphicsProvider.getColor(ChartColor.BLACK),LineStyle.LINE_NORMAL);
		  data1.valueFont=GraphicsProvider.getFont("Arial",ChartFont.PLAIN,10);

		  double[] d2={-2,3,4,4.2,6.4,4.5,6.1};
		  BarDataSerie data2= new BarDataSerie(d2,new FillStyle(GraphicsProvider.getColor(ChartColor.GREEN)));
		  data2.valueFont=GraphicsProvider.getFont("Arial",ChartFont.PLAIN,10);
		  data2.border=new LineStyle(1,GraphicsProvider.getColor(ChartColor.BLACK),LineStyle.LINE_NORMAL);
		  data2.negativeStyle=new FillStyle(GraphicsProvider.getColor(ChartColor.RED));			  

		  Legend l=new Legend();
		  l.border=new LineStyle(1,GraphicsProvider.getColor(ChartColor.BLACK),LineStyle.LINE_NORMAL);
		  l.addItem("Company A",new FillStyle(GraphicsProvider.getColor(ChartColor.CYAN)));
		  l.addItem("Company B",new FillStyle(GraphicsProvider.getColor(ChartColor.GREEN)));

		  // create title
		  Title title=new Title("Benefits companies A & B");
		  // create axis
		  Axis  XAxis=new Axis(Axis.HORIZONTAL,new Scale());
		  Axis  YAxis=new Axis(Axis.VERTICAL,new Scale());
		  XAxis.tickAtBase=true; // draw also first tick
		  XAxis.scale.min=-3;
		  XAxis.scale.max=7;
		  YAxis.scale.min=0;
		  YAxis.scale.max=7;
		  YAxis.IntegerScale=true;
		  YAxis.scaleTickInterval=1;
		  XAxis.scaleTickInterval=1;
		  XAxis.gridStyle=new LineStyle(1,GraphicsProvider.getColor(ChartColor.WHITE),LineStyle.LINE_DOTS);
		  YAxis.gridStyle=new LineStyle(1,GraphicsProvider.getColor(ChartColor.WHITE),LineStyle.LINE_DOTS);
		  String[] lbls={"1999","2000","2001","2002","2003","2004","2005","2006"};
		  YAxis.tickLabels=lbls;

		  HAxisLabel XLabel= new HAxisLabel("million $",GraphicsProvider.getColor(ChartColor.BLACK),GraphicsProvider.getFont("Arial",ChartFont.PLAIN,12));
		  VAxisLabel YLabel= new VAxisLabel("Year",GraphicsProvider.getColor(ChartColor.BLACK),GraphicsProvider.getFont("Arial",ChartFont.PLAIN,12));
		  YLabel.vertical=true;

		  // plotter
		  BarPlotter plot=new BarPlotter();
		  plot.verticalBars=false;
		  plot.interBarSpace=0;
		  // create report
		  Chart chart=new Chart(title,plot,XAxis,YAxis);
		  chart.XLabel=XLabel;
		  chart.YLabel=YLabel;
		  chart.legend=l;
		  chart.addSerie(data2);
		  chart.addSerie(data1);
		  chart.back=new FillStyle(GraphicsProvider.getColor(ChartColor.LIGHTGRAY));

		  return chart;

		}

		/**
		 * Line chart example
		 * @return
		 */
		public static  Chart Example1() {

		   // data
		  double[] d1={1,0,3,4,5,4,2};
		  double[] d2={3,8,4,3,0,9,6.100002};

		  // series
		  LineDataSerie data1= new LineDataSerie(d1,new LineStyle(2,GraphicsProvider.getColor(ChartColor.BLUE),LineStyle.LINE_NORMAL));
		  data1.drawPoint=true;
		  data1.valueFormat="###.0";
		  data1.valueFont=GraphicsProvider.getFont("Arial",ChartFont.PLAIN,10);
		  LineDataSerie data2= new LineDataSerie(d2,new LineStyle(2,GraphicsProvider.getColor(ChartColor.GREEN),LineStyle.LINE_NORMAL));
		  data2.drawPoint=true;
		  //data2.fillStyle=new FillStyle(GraphicsProvider.getColor(ChartColor.GREEN));
		  data2.fillStyle=new FillStyle(GraphicsProvider.getColor(ChartColor.BLUE),0.5f );
		  data2.valueFormat="###.0";
		  data2.valueFont=GraphicsProvider.getFont("Arial",ChartFont.PLAIN,10);

		  // legend
		  Legend l=new Legend();
		  l.background=new FillStyle(GraphicsProvider.getColor(ChartColor.WHITE));
		  l.border=new LineStyle(1,GraphicsProvider.getColor(ChartColor.BLACK),LineStyle.LINE_NORMAL);
		  l.addItem("Products",new LineStyle(1,GraphicsProvider.getColor(ChartColor.BLUE),LineStyle.LINE_NORMAL));
		  l.addItem("Services",new LineStyle(1,GraphicsProvider.getColor(ChartColor.GREEN),LineStyle.LINE_NORMAL));
			  

		  // create title
		  Title title=new Title("Sales (thousands $)");
		  // create axis
		  Axis  XAxis=new Axis(Axis.HORIZONTAL,new Scale());
		  Axis  YAxis=new Axis(Axis.VERTICAL,new Scale());
		  XAxis.tickAtBase=true; // draw also first tick
		  XAxis.scale.min=0;
		  YAxis.scale.min=0;
		  YAxis.scale.max=7;
		  YAxis.scaleTickInterval=1;
		  XAxis.scaleTickInterval=1;
		  XAxis.gridStyle=new LineStyle(1,GraphicsProvider.getColor(ChartColor.WHITE),LineStyle.LINE_DOTS);
		  YAxis.gridStyle=new LineStyle(1,GraphicsProvider.getColor(ChartColor.WHITE),LineStyle.LINE_DOTS);
		  XAxis.tickLabels=lbls;


		  HAxisLabel XLabel= new HAxisLabel("Date",GraphicsProvider.getColor(ChartColor.WHITE),GraphicsProvider.getFont("Arial",ChartFont.PLAIN,14));
		  VAxisLabel YLabel= new VAxisLabel("Brutto",GraphicsProvider.getColor(ChartColor.WHITE),GraphicsProvider.getFont("Arial",ChartFont.PLAIN,14));
		  YLabel.vertical=true;

		  // plotter
		  LinePlotter plot=new LinePlotter();
		  plot.fixedLimits=true;

		  // create report
		  Chart chart=new Chart(title,plot,XAxis,YAxis);
		  chart.back=new FillStyle(GraphicsProvider.getColor(ChartColor.RED));
		  chart.back.gradientType=FillStyle.GRADIENT_HORIZONTAL;
		  chart.XLabel=XLabel;
		  chart.YLabel=YLabel;
		  chart.legend=l;
		  chart.addSerie(data2);
		  chart.addSerie(data1);
	  
		  return chart;
	 }


	/**
	 * Radar chart (transparent filling) example
	 * @return
	 */
		public static  Chart Example12() {

		   // data
		  double[] d1={1,2,3,4,5};
		  double[] d2={2,3,4,4.2,3};

			  // series
		  LineDataSerie data1= new LineDataSerie(d1,new LineStyle(2,GraphicsProvider.getColor(ChartColor.BLUE),LineStyle.LINE_NORMAL));
		  data1.drawPoint=true;
		  data1.valueFont=GraphicsProvider.getFont("Arial",ChartFont.PLAIN,10);
		  data1.fillStyle=new FillStyle(GraphicsProvider.getColor(ChartColor.BLUE),0.5f);
		  
		  LineDataSerie data2= new LineDataSerie(d2,new LineStyle(2,GraphicsProvider.getColor(ChartColor.GREEN),LineStyle.LINE_NORMAL));
		  data2.drawPoint=true;
		  data2.valueFont=GraphicsProvider.getFont("Arial",ChartFont.PLAIN,10);
		  data2.fillStyle=new FillStyle(GraphicsProvider.getColor(ChartColor.GREEN),0.5f);

		  // legend
		  Legend l=new Legend();
		  l.background=new FillStyle(GraphicsProvider.getColor(ChartColor.WHITE));
		  l.border=new LineStyle(1,GraphicsProvider.getColor(ChartColor.BLACK),LineStyle.LINE_NORMAL);
		  l.addItem("Products",new LineStyle(1,GraphicsProvider.getColor(ChartColor.BLUE),LineStyle.LINE_NORMAL));
		  l.addItem("Services",new LineStyle(1,GraphicsProvider.getColor(ChartColor.GREEN),LineStyle.LINE_NORMAL));

		  // create title
		  Title title=new Title("Sales (thousands $)");

		  // plotter
		  RadarPlotter plot=new RadarPlotter();

		  double[] fMaxs={6,6,6,6,6};
		  double[] fMins={0,0,0,0,0};
		  String[] factors={"factor1","factor2","factor3","factor4","factor5","factor6"};

		  plot.factorMaxs=fMaxs;
		  plot.factorMins=fMins;
		  plot.factorNames=factors;
		  plot.backStyle=new FillStyle(GraphicsProvider.getColor(ChartColor.YELLOW));
		  plot.radiusModifier=0.8;

		  plot.gridStyle=new LineStyle(1,GraphicsProvider.getColor(ChartColor.BLACK),LineStyle.LINE_DASHED);
		  plot.gridFont=GraphicsProvider.getFont("Arial",ChartFont.PLAIN,10);

		  // create report
		  Chart chart=new Chart(title,plot,null,null);
		  chart.back=new FillStyle(GraphicsProvider.getColor(ChartColor.RED));
		  chart.back.gradientType=FillStyle.GRADIENT_HORIZONTAL;
		  chart.legend=l;
		  chart.addSerie(data2);
		  chart.addSerie(data1);

		   return chart;
	 }


	/**
	 * area chart example
	 * @return
	 */
	public static  Chart Example5() {

		   // data
		  double[] d1={3,4,3,4,5,4,2};
		  double[] d2={2,3,4,6,6.4,4.5,6.1};

		  // series
		  LineDataSerie data1= new LineDataSerie(d1,new LineStyle(1,GraphicsProvider.getColor(ChartColor.BLACK),LineStyle.LINE_NORMAL));
		  data1.drawPoint=true;
		  data1.valueFont=GraphicsProvider.getFont("Arial",ChartFont.PLAIN,10);
		  data1.fillStyle=new FillStyle(GraphicsProvider.getColor(ChartColor.BLUE),0.5f);
		  LineDataSerie data2= new LineDataSerie(d2,new LineStyle(1,GraphicsProvider.getColor(ChartColor.BLACK),LineStyle.LINE_NORMAL));
		  data2.drawPoint=true;
		  data2.valueFont=GraphicsProvider.getFont("Arial",ChartFont.PLAIN,10);
		  data2.fillStyle=new FillStyle(GraphicsProvider.getColor(ChartColor.GREEN));

		  // legend
		  Legend l=new Legend();
		  l.addItem("Products",new FillStyle(GraphicsProvider.getColor(ChartColor.BLUE)));
		  l.addItem("Services",new FillStyle(GraphicsProvider.getColor(ChartColor.GREEN)));

		  // create title
		  Title title=new Title("Sales (thousands $)");
		  // create axis
		  Axis  XAxis=new Axis(Axis.HORIZONTAL,new Scale());
		  Axis  YAxis=new Axis(Axis.VERTICAL,new Scale());
		  XAxis.tickAtBase=true; // draw also first tick
		  XAxis.scale.min=0;
		  YAxis.scale.min=0;
		  XAxis.autoNumberOfTicks=5;
		  double[] pti={0.1,0.5,1,2,3,4,5,7,10,25,50,100,250,500,1000,5000,10000,50000,100000,500000,1000000};
		  XAxis.ticks_preferred_Interval=pti;

		  YAxis.scaleTickInterval=1;
		  XAxis.scaleTickInterval=1;
		  YAxis.bigTickInterval=2;
		  YAxis.gridStyle=new LineStyle(1,GraphicsProvider.getColor(ChartColor.GRAY),LineStyle.LINE_NORMAL);

		  HAxisLabel XLabel= new HAxisLabel("",GraphicsProvider.getColor(ChartColor.BLUE),GraphicsProvider.getFont("Arial",ChartFont.PLAIN,14));
		  VAxisLabel YLabel= new VAxisLabel("Brutto",GraphicsProvider.getColor(ChartColor.BLACK),GraphicsProvider.getFont("Arial",ChartFont.PLAIN,14));

		  // plotter
		  LinePlotter plot=new LinePlotter();

		  // create report
		  Chart chart=new Chart(title,plot,XAxis,YAxis);
		  chart.back=new FillStyle(GraphicsProvider.getColor(ChartColor.YELLOW));
		  chart.back.colorFrom=GraphicsProvider.getColor(ChartColor.YELLOW);
		  chart.back.colorUntil=GraphicsProvider.getColor(ChartColor.WHITE);
		  chart.back.gradientType=FillStyle.GRADIENT_HORIZONTAL;
		  chart.XLabel=XLabel;
		  chart.YLabel=YLabel;
		  chart.legend=l;
		  chart.addSerie(data2);
		  chart.addSerie(data1);

		  return chart;
		}


	/**
	 * Column chart example
	 * @return
	 */
		public static  Chart Example6() {

		  // data
		  double[] d1={1,2,3,4,5,4,2};
		  BarDataSerie data1= new BarDataSerie(d1,new FillStyle(GraphicsProvider.getColor(ChartColor.ORANGE)));
		  data1.borderType=BarDataSerie.BORDER_RAISED;
		  data1.valueFont=GraphicsProvider.getFont("Arial",ChartFont.PLAIN,10);

		  double[] d2={2,3,4,4.2,6.4,4.5,6.1};
		  BarDataSerie data2= new BarDataSerie(d2,new FillStyle(GraphicsProvider.getColor(ChartColor.GREEN)));
		  data2.valueFont=GraphicsProvider.getFont("Arial",ChartFont.PLAIN,10);
		  data2.borderType=BarDataSerie.BORDER_RAISED;
		  data2.negativeStyle=new FillStyle(GraphicsProvider.getColor(ChartColor.RED));

		  Legend l=new Legend();
		  l.border=new LineStyle(1,GraphicsProvider.getColor(ChartColor.BLACK),LineStyle.LINE_NORMAL);
		  l.addItem("Company A",new FillStyle(GraphicsProvider.getColor(ChartColor.ORANGE)));
		  l.addItem("Company B",new FillStyle(GraphicsProvider.getColor(ChartColor.GREEN)));
		  l.background=new FillStyle(GraphicsProvider.getColor(ChartColor.LIGHTGRAY));

		  // create title
		  Title title=new Title("Benefits companies A & B");
		  // create axis
		  Axis  XAxis=new Axis(Axis.HORIZONTAL,new Scale());
		  Axis  YAxis=new Axis(Axis.VERTICAL,new Scale());
		  XAxis.tickAtBase=true; // draw also first tick
		  XAxis.scale.min=0;
		  XAxis.scale.max=7;
		  YAxis.scale.min=0;
		  YAxis.scale.max=7;
		  YAxis.IntegerScale=true;
		  YAxis.scaleTickInterval=1;
		  XAxis.scaleTickInterval=1;
		  XAxis.gridStyle=new LineStyle(1,GraphicsProvider.getColor(ChartColor.WHITE),LineStyle.LINE_DOTS);
		  YAxis.gridStyle=new LineStyle(1,GraphicsProvider.getColor(ChartColor.WHITE),LineStyle.LINE_DOTS);
		  String[] lbls={"1999","2000","2001","2002","2003","2004","2005","2006"};
		  XAxis.tickLabels=lbls;

		  HAxisLabel XLabel= new HAxisLabel("Year",GraphicsProvider.getColor(ChartColor.BLACK),GraphicsProvider.getFont("Arial",ChartFont.PLAIN,12));
		  VAxisLabel YLabel= new VAxisLabel("million $",GraphicsProvider.getColor(ChartColor.BLACK),GraphicsProvider.getFont("Arial",ChartFont.PLAIN,12));

		  // plotter
		  BarPlotter plot=new BarPlotter();
		  // create report
		  Chart chart=new Chart(title,plot,XAxis,YAxis);
		  plot.interBarSpace=1;
		  chart.XLabel=XLabel;
		  chart.YLabel=YLabel;
		  chart.legend=l;
		  chart.addSerie(data2);
		  chart.addSerie(data1);
		  chart.back=new FillStyle(GraphicsProvider.getColor(ChartColor.LIGHTGRAY));
		  chart.backImage=GraphicsProvider.getImage("back13.gif");

		  return chart;

		}

		/**
		 * stacked bars example
		 * @return
		 */
		public static  Chart Example7() {

		  // data
		  double[] d1={1,2,3,4,5,4,2};
		  BarDataSerie data1= new BarDataSerie(d1,new FillStyle(GraphicsProvider.getColor(ChartColor.BLUE)));
		  data1.borderType=BarDataSerie.BORDER_RAISED;

		  double[] d2={2,3,4,1.2,1.4,1.5,3.1};
		  BarDataSerie data2= new BarDataSerie(d2,new FillStyle(GraphicsProvider.getColor(ChartColor.RED)));
		  data2.valueFont=GraphicsProvider.getFont("Arial",ChartFont.PLAIN,10);
		  data2.borderType=BarDataSerie.BORDER_RAISED;


		  Legend l=new Legend();
		  l.addItem("Company A",new FillStyle(GraphicsProvider.getColor(ChartColor.RED)));
		  l.addItem("Company B",new FillStyle(GraphicsProvider.getColor(ChartColor.BLUE)));

		  // create title
		  Title title=new Title("Benefits companies A + B");
		  // create axis
		  Axis  XAxis=new Axis(Axis.HORIZONTAL,new Scale());
		  Axis  YAxis=new Axis(Axis.VERTICAL,new Scale());
		  XAxis.tickAtBase=true; // draw also first tick
		  XAxis.scale.min=0;
		  XAxis.scale.max=8;
		  YAxis.scale.min=0;
		  YAxis.scale.max=7;
		  YAxis.IntegerScale=true;
		  YAxis.scaleTickInterval=1;
		  XAxis.scaleTickInterval=1;
		  XAxis.style=new LineStyle(2,GraphicsProvider.getColor(ChartColor.LIGHTGRAY),LineStyle.LINE_NORMAL);
		  YAxis.style=new LineStyle(2,GraphicsProvider.getColor(ChartColor.LIGHTGRAY),LineStyle.LINE_NORMAL);
		  XAxis.gridStyle=new LineStyle(1,GraphicsProvider.getColor(ChartColor.LIGHTGRAY),LineStyle.LINE_DASHED);
		  YAxis.gridStyle=new LineStyle(1,GraphicsProvider.getColor(ChartColor.LIGHTGRAY),LineStyle.LINE_DASHED);
		  String[] lbls={" ","1999","2000","2001","2002","2003","2004","2005"," "};
		  XAxis.tickLabels=lbls;

		  HAxisLabel XLabel= new HAxisLabel("Year",GraphicsProvider.getColor(ChartColor.BLACK),GraphicsProvider.getFont("Arial",ChartFont.PLAIN,12));
		  VAxisLabel YLabel= new VAxisLabel("million $",GraphicsProvider.getColor(ChartColor.BLACK),GraphicsProvider.getFont("Arial",ChartFont.PLAIN,12));

		  // plotter
		  BarPlotter plot=new BarPlotter();
		  // create report
		  Chart chart=new Chart(title,plot,XAxis,YAxis);
		  chart.XLabel=XLabel;
		  chart.YLabel=YLabel;
		  chart.legend=l;
		  chart.addSerie(data2);
		  chart.addSerie(data1);
		  chart.back=new FillStyle(GraphicsProvider.getColor(ChartColor.LIGHTGRAY));
		  plot.cumulative=true;
		  plot.back=new FillStyle(GraphicsProvider.getColor(ChartColor.WHITE));

		  return chart;
		}

	/**
	 * Combined lines and bars example
	 * @return
	 */
	 public static  Chart Example10() {

		   // data
		  double[] d1={1,2,3,4,5,3,4};
		  double[] d2={0,1,2,3,4,5,3,4};

		  // series
		  LineDataSerie data2= new LineDataSerie(d2,new LineStyle(2,GraphicsProvider.getColor(ChartColor.BLUE),LineStyle.LINE_NORMAL));
		  data2.valueFont=GraphicsProvider.getFont("Arial",ChartFont.PLAIN,10);
		  BarDataSerie data1= new BarDataSerie(d1,new FillStyle(GraphicsProvider.getColor(ChartColor.ORANGE)));
		  data1.border=new LineStyle(1,GraphicsProvider.getColor(ChartColor.BLACK),LineStyle.LINE_NORMAL);

		 // legend
		  Legend l=new Legend();
		  l.background=new FillStyle(GraphicsProvider.getColor(ChartColor.WHITE));
		  l.border=new LineStyle(1,GraphicsProvider.getColor(ChartColor.BLACK),LineStyle.LINE_NORMAL);
		  l.addItem("Products",new LineStyle(1,GraphicsProvider.getColor(ChartColor.BLUE),LineStyle.LINE_NORMAL));
		  l.verticalLayout=false;

		  // create title
		  Title title=new Title("Sales (thousands $)");
		  // create axis
		  Axis  XAxis=new Axis(Axis.HORIZONTAL,new Scale());
		  Axis  YAxis=new Axis(Axis.VERTICAL,new Scale());
		  XAxis.tickAtBase=true; // draw also first tick
		  XAxis.scale.min=0;
		  YAxis.scale.min=0;
		  YAxis.scale.max=7;
		  YAxis.scaleTickInterval=1;
		  XAxis.scaleTickInterval=1;
		  String[] lbls={" ","June","July","Aug.","Sept.","Oct.","Nov.","Dec."};
		  XAxis.tickLabels=lbls;


		  HAxisLabel XLabel= new HAxisLabel("",GraphicsProvider.getColor(ChartColor.BLUE),GraphicsProvider.getFont("Arial",ChartFont.PLAIN,14));
		  VAxisLabel YLabel= new VAxisLabel("Brutto",GraphicsProvider.getColor(ChartColor.WHITE),GraphicsProvider.getFont("Arial",ChartFont.PLAIN,14));

		  // plotter
		  LinePlotter plot=new LinePlotter();

		 // second plotter
		  BarPlotter plot2=new BarPlotter();

		  // create report
		  Chart chart=new Chart(title,plot2,XAxis,YAxis);
		  chart.addPlotter(plot);
		  chart.back=new FillStyle(GraphicsProvider.getColor(ChartColor.GREEN));
		  chart.back.colorUntil=GraphicsProvider.getColor(ChartColor.GREEN);
		  chart.back.colorFrom=GraphicsProvider.getColor(ChartColor.WHITE);
		  chart.back.gradientType=FillStyle.GRADIENT_VERTICAL;
		  chart.XLabel=XLabel;
		  chart.YLabel=YLabel;
		  chart.legend=l;
		  chart.layout=Chart.LAYOUT_LEGEND_BOTTOM;
		  plot.addSerie(data2);
		  plot2.addSerie(data1);
		  plot2.barWidth=20;

		  return chart;

		}


		/**
		 * Max/Min example
		 * @return
		 */
		public static  Chart Example14() {

		   // data
		  double[] d1={2,1 ,2 ,3 ,4 ,5 ,4 ,3};
		  double[] dmin1={1.5,0.85 ,1.5 ,2.8 ,3, 4.8 ,3.5,2.5 };
		  double[] dmax1={2.3,1.15 ,2.5 ,3.2 ,4.4 ,5.3,4.5 ,3.3};
		  MaxMinDataSerie data1= new MaxMinDataSerie(d1,null);
		  data1.drawPoint=true;
		  data1.drawLineEnd=true;
		  data1.pointColor=GraphicsProvider.getColor(ChartColor.GREEN);
		  data1.setMaxMinValues(dmax1,dmin1);
		  data1.maxminStyle=new LineStyle(1,GraphicsProvider.getColor(ChartColor.WHITE),LineStyle.LINE_NORMAL);
		
		  double[] d2={2,1 ,2 ,3 ,4 ,5 ,4 ,3};
		  LineDataSerie data2= new LineDataSerie(d2,new LineStyle(1,GraphicsProvider.getColor(ChartColor.WHITE),LineStyle.LINE_DOTS));
		  data2.valueFont=GraphicsProvider.getFont("Arial",ChartFont.PLAIN,10);
		  data2.valueColor=GraphicsProvider.getColor(ChartColor.GREEN);

		  // create title
		  Title title=new Title("Price");
		  title.color=GraphicsProvider.getColor(ChartColor.WHITE);

		  // create axis
		  Axis  XAxis=new Axis(Axis.HORIZONTAL,new Scale());
		  Axis  YAxis=new Axis(Axis.VERTICAL,new Scale());
		  XAxis.tickAtBase=true; // draw also first tick
		  XAxis.scale.min=0;
		  YAxis.scale.min=0;
		  YAxis.scale.max=6;
		  YAxis.DescColor=GraphicsProvider.getColor(ChartColor.WHITE);
		  YAxis.scaleTickInterval=1;
		  XAxis.scaleTickInterval=1;
		  XAxis.bigTickInterval=1;
		  XAxis.DescColor=GraphicsProvider.getColor(ChartColor.WHITE);
		  String[] lbls={"8 Jan.","9 Jan.","10 Jan.","11 Jan.","12 Jan.","13 Jan.","14 Jan.","15 Jan."};
		  XAxis.tickLabels=lbls;
		  XAxis.style =new LineStyle(1,GraphicsProvider.getColor(ChartColor.WHITE),LineStyle.LINE_NORMAL);
		  YAxis.style =new LineStyle(1,GraphicsProvider.getColor(ChartColor.WHITE),LineStyle.LINE_NORMAL);

		  HAxisLabel XLabel= new HAxisLabel("Week",GraphicsProvider.getColor(ChartColor.WHITE),GraphicsProvider.getFont("Arial",ChartFont.PLAIN,12));
		  VAxisLabel YLabel= new VAxisLabel("Value",GraphicsProvider.getColor(ChartColor.WHITE),GraphicsProvider.getFont("Arial",ChartFont.PLAIN,12));

		  // plotter
		  LinePlotter plot=new LinePlotter();

		  // create report
		  Chart chart=new Chart(title,plot,XAxis,YAxis);
		  chart.XLabel=XLabel;
		  chart.YLabel=YLabel;
		  chart.addSerie(data2);
		  chart.addSerie(data1);
		  chart.back=new FillStyle(GraphicsProvider.getColor(ChartColor.BLACK));

		  chart.backImage=GraphicsProvider.getImage("back5.jpg");

		  return chart;

		}



	/**
	 * Curves and least squares line example
	 * @return
	 */
	public static  Chart Example15() {

	 	ChartImage im1=GraphicsProvider.getImage("point.gif");

		ChartImage im2=GraphicsProvider.getImage("point2.gif");
		// load backgorund
		ChartImage im3=GraphicsProvider.getImage("back19.jpg");

		   // data
		double[] d1={1,4,3,4,5,4,2};
	    double[] d2={2,3,4,6,6.4,4.5,6.1};

		// series
		LineDataSerie data1= new LineDataSerie(d1,new LineStyle(1,GraphicsProvider.getColor(ChartColor.BLUE),LineStyle.LINE_NORMAL));
		data1.drawPoint=true;
		data1.icon=im1;
		data1.lineType=LinePlotter.TYPE_CUBIC_NATURAL;


		LineDataSerie data2= new LineDataSerie(d2,new LineStyle(2,GraphicsProvider.getColor(ChartColor.GREEN),LineStyle.LINE_DASHED));
		data2.drawPoint=true;
		data2.icon=im2;
		data2.lineType=LinePlotter.TYPE_LEAST_SQUARES_LINE;

		// legend
		Legend l=new Legend();
		l.addItem("Products",im1);
		l.addItem("Services",im2);

		// create title
		Title title=new Title("Sales (thousands $)");
		// create axis
		Axis  XAxis=new Axis(Axis.HORIZONTAL,new Scale());
		Axis  YAxis=new Axis(Axis.VERTICAL,new Scale());
		XAxis.tickAtBase=true; // draw also first tick
		XAxis.scale.min=0;
		YAxis.scale.min=0;
		YAxis.scaleTickInterval=1;
		XAxis.scaleTickInterval=1;
		YAxis.bigTickInterval=2;
		YAxis.gridStyle=new LineStyle(1,GraphicsProvider.getColor(ChartColor.BLACK),LineStyle.LINE_NORMAL);
		String[] lbls={"June","July","Aug.","Sept.","Oct.","Nov.","Dec."};
		XAxis.tickLabels=lbls;


		HAxisLabel XLabel= new HAxisLabel("",GraphicsProvider.getColor(ChartColor.BLUE),GraphicsProvider.getFont("Arial",ChartFont.PLAIN,14));
		VAxisLabel YLabel= new VAxisLabel("Brutto",GraphicsProvider.getColor(ChartColor.BLACK),GraphicsProvider.getFont("Arial",ChartFont.PLAIN,14));

		// plotter
		CurvePlotter plot=new CurvePlotter();

		// create report
		Chart chart=new Chart(title,plot,XAxis,YAxis);
		chart.back=new FillStyle(GraphicsProvider.getColor(ChartColor.YELLOW));
		chart.back.colorFrom=GraphicsProvider.getColor(ChartColor.YELLOW);
		chart.back.colorUntil=GraphicsProvider.getColor(ChartColor.WHITE);
		chart.back.gradientType=FillStyle.GRADIENT_HORIZONTAL;
		chart.XLabel=XLabel;
		chart.YLabel=YLabel;
		chart.legend=l;
		chart.addSerie(data2);
		chart.addSerie(data1);
		chart.backImage=im3;

		return chart;

	 }

	/**
	 * OCHL chart example
	 * @return
	 */
	  public static  Chart Example11() {

		// data
		double[] d1=      {2   ,1    ,2  ,3  ,4.3 ,5   ,4   ,3};
		double[] dclose1={ 2.3 , 1.15 ,2.5,2.8,4 ,5.1 ,3.5 ,3.2};
		double[] dmin1={1.5    , 0.85,1.5,2.8,3 , 4.8,3.0 ,2.5 };
		double[] dmax1={2.5    ,1.5 ,2.6,3.2, 4.4,5.3,4.5 ,3.3};
		MaxMinDataSerie data1= new MaxMinDataSerie(d1,dclose1,dmin1,dmax1,null);
		data1.drawLineEnd=false;
		data1.setMaxMinValues(dmax1,dmin1);
		data1.maxminStyle=new LineStyle(1,GraphicsProvider.getColor(ChartColor.BLACK),LineStyle.LINE_NORMAL);
		data1.drawPoint=false;
		data1.positiveValueColor=GraphicsProvider.getColor(ChartColor.YELLOW);

		// create title
		Title title=new Title("Price");
		title.color=GraphicsProvider.getColor(ChartColor.BLACK);

		// create axis
		Axis  XAxis=new Axis(Axis.HORIZONTAL,new Scale());
		Axis  YAxis=new Axis(Axis.VERTICAL,new Scale());
		XAxis.tickAtBase=false; // draw also first tick
		YAxis.ceroAxis=Axis.CEROAXIS_NO;
		YAxis.gridStyle=new LineStyle(1,GraphicsProvider.getColor(ChartColor.WHITE),LineStyle.LINE_DASHED);
		XAxis.scale.min=-1;
		YAxis.scale.min=0;
		YAxis.scale.max=6;
		YAxis.DescColor=GraphicsProvider.getColor(ChartColor.BLACK);
		YAxis.scaleTickInterval=1;
		XAxis.scaleTickInterval=1;
		XAxis.bigTickInterval=1;
		XAxis.gridStyle=new LineStyle(1,GraphicsProvider.getColor(ChartColor.WHITE),LineStyle.LINE_DASHED);
		XAxis.DescColor=GraphicsProvider.getColor(ChartColor.BLACK);
		String[] lbls={"8 Jan.","9 Jan.","10 Jan.","11 Jan.","12 Jan.","13 Jan.","14 Jan.","15 Jan."};
		XAxis.tickLabels=lbls;
		XAxis.style =new LineStyle(2,GraphicsProvider.getColor(ChartColor.BLACK),LineStyle.LINE_NORMAL);
		YAxis.style =new LineStyle(2,GraphicsProvider.getColor(ChartColor.BLACK),LineStyle.LINE_NORMAL);

		HAxisLabel XLabel= new HAxisLabel("Week",GraphicsProvider.getColor(ChartColor.BLACK),GraphicsProvider.getFont("Arial",ChartFont.PLAIN,12));
		VAxisLabel YLabel= new VAxisLabel("Value",GraphicsProvider.getColor(ChartColor.BLACK),GraphicsProvider.getFont("Arial",ChartFont.PLAIN,12));

		// plotter
		LinePlotter plot=new LinePlotter();
		plot.MaxMinType=LinePlotter.MM_CANDLESTICK;

		// create report
		Chart chart=new Chart(title,plot,XAxis,YAxis);
		chart.XLabel=XLabel;
		chart.YLabel=YLabel;
		chart.addSerie(data1);
		chart.back=new FillStyle(GraphicsProvider.getColor(ChartColor.BLACK));

		chart.backImage=GraphicsProvider.getImage("back13.gif");

		return chart;

	}
	
	/**
	 * create default chart to show 
	 * @return
	 */
	  public static Chart getDefaultChart() {

		ChartLoader cha=new ChartLoader();			
		cha.clearParams();	
		cha.setParameter("TITLECHART","Sales");
		cha.setParameter("XLABEL","Month");
		cha.setParameter("YLABEL","Million $");
		cha.setParameter("XSCALE_MIN","0");
		cha.setParameter("XSCALE_MAX","5.5");
		cha.setParameter("YSCALE_MIN","-15");
		cha.setParameter("BIG_TICK_INTERVALX","1");
		cha.setParameter("BIG_TICK_INTERVALY","10");
		cha.setParameter("XAXIS_LABELS","June|July|Aug.|Sept.|Oct.|Nov.|Dec.");
		cha.setParameter("CERO_XAXIS","LINE");
		cha.setParameter("YAXIS_INTEGER","TRUE");
		cha.setParameter("SERIE_1","Products");
		cha.setParameter("SERIE_2","Services");
		cha.setParameter("SERIE_TYPE_1","BAR");
		cha.setParameter("SERIE_TYPE_2","BAR");
		cha.setParameter("SERIE_FONT_1","Arial|PLAIN|8");
		cha.setParameter("SERIE_FONT_2","Arial|BOLD|8");
		cha.setParameter("BOTTOM_MARGIN","0.18");
		cha.setParameter("SERIE_DATA_2","-10|41|48|39|36");
		cha.setParameter("SERIE_BORDER_TYPE_1","RAISED");
		cha.setParameter("SERIE_BORDER_TYPE_2","RAISED");
		cha.setParameter("SERIE_BAR_STYLE_1","0x00FF00");
		cha.setParameter("SERIE_BAR_STYLE_2","0x0000FF");
		cha.setParameter("BARCHART_BARSPACE","1");
		cha.setParameter("LEFT_MARGIN","0.15");
		cha.setParameter("CHART_FILL","0xFFCC00");
		cha.setParameter("SERIE_NEGATIVE_STYLE_2","RED");
		cha.setParameter("YLABEL_VERTICAL","TRUE");
		cha.setParameter("SERIE_DATA_1","12|43|50|45|30");
		cha.setParameter("SERIE_TIPS_1","12|43|50|45|30");
		cha.setParameter("SERIE_TIPS_2","-10|41|48|39|36");
		cha.setParameter("CHART_SHOW_TIPS","true");
		cha.setParameter("BARCHART_BARSPACE","5");						
		cha.setParameter("CHART_WIDTH","700");
		//cha.setParameter("CHART_HEIGHT","700");							
	
		return cha.build(false,false);
	
		}
	


}
