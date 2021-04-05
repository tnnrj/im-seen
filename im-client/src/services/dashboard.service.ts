import { DashElement, DashPage } from "@/model/dashboard.model";
import { ChartType, DashLayout } from "@/model/enums.model";

export function getDashPages(): DashPage[] {
  // TODO: get user pages from API
  return [
    dashLayoutToDefaultPage(DashLayout.ThreeOneTB),
    dashLayoutToDefaultPage(DashLayout.TwoTB),
    dashLayoutToDefaultPage(DashLayout.ThreeTwoLR),
  ];
}

export function dashLayoutToDefaultPage(layout: DashLayout): DashPage {
  let elements: DashElement[] = [];
  switch(layout) {
    case DashLayout.One:
      elements.push({
        queryId: '',
        chartType: ChartType.None,
        width: 100,
        height: 100
      });
      break;
    case DashLayout.TwoLR:
      elements = elements.concat(dashElementRow(1, true), dashElementRow(1, true));
      break;
    case DashLayout.TwoTB:
      elements = elements.concat(dashElementRow(1), dashElementRow(1));
      break;
    case DashLayout.ThreeOneLR:
      elements = elements.concat(dashElementRow(1, true), dashElementRow(2, true));
      break;
    case DashLayout.ThreeOneTB:
      elements = elements.concat(dashElementRow(1), dashElementRow(2));
      break;
    case DashLayout.ThreeTwoLR:
      elements = elements.concat(dashElementRow(2, true), dashElementRow(1, true));
      break;
    case DashLayout.ThreeTwoTB:
      elements = elements.concat(dashElementRow(2), dashElementRow(1));
      break;
    case DashLayout.Four:
      elements = elements.concat(dashElementRow(2), dashElementRow(2));
      break;
    case DashLayout.FourOneLR:
      elements = elements.concat(dashElementRow(1, true), dashElementRow(3, true));
      break;
    case DashLayout.FourOneTB:
      elements = elements.concat(dashElementRow(1), dashElementRow(3));
      break;
    case DashLayout.FourThreeLR:
      elements = elements.concat(dashElementRow(3, true), dashElementRow(1, true));
      break;
    case DashLayout.FourThreeTB:
      elements = elements.concat(dashElementRow(3), dashElementRow(1));
      break;
    case DashLayout.FiveTwoLR:
      elements = elements.concat(dashElementRow(2, true), dashElementRow(3, true));
      break;
    case DashLayout.FiveTwoTB:
      elements = elements.concat(dashElementRow(2), dashElementRow(3));
      break;
    case DashLayout.FiveThreeLR:
      elements = elements.concat(dashElementRow(3, true), dashElementRow(2, true));
      break;
    case DashLayout.FiveThreeTB:
      elements = elements.concat(dashElementRow(3), dashElementRow(2));
      break;
  }
  return { layout, elements };
}

function dashElementRow(count: number, col = false): DashElement[] {
  let elements: DashElement[] = [];
  for (let i = 0; i < count; i++) {
    elements.push({
      queryId: '',
      chartType: ChartType.BubbleCloud,
      width: col ? 50 : 100 / count,
      height: col ? 100 / count : 50
    });
  }
  return elements;
}

export function dashLayoutToPanelCount(layout: DashLayout): number {
  switch(layout) {
    case DashLayout.One:
      return 1;
    case DashLayout.TwoLR:
    case DashLayout.TwoTB:
      return 2;
    case DashLayout.ThreeOneLR:
    case DashLayout.ThreeOneTB:
    case DashLayout.ThreeTwoLR:
    case DashLayout.ThreeTwoTB:
      return 3;
    case DashLayout.Four:
    case DashLayout.FourOneLR:
    case DashLayout.FourOneTB:
    case DashLayout.FourThreeLR:
    case DashLayout.FourThreeTB:
      return 4;
    case DashLayout.FiveTwoLR:
    case DashLayout.FiveTwoTB:
    case DashLayout.FiveThreeLR:
    case DashLayout.FiveThreeTB:
      return 5;
  }
  return 0;
}
