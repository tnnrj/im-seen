import { DashElement, DashPage } from "@/model/dashboard.model";
import { ChartType, DashLayout } from "@/model/enums.model";
import http from "@/services/base-api.service";

export default {
  getDashPages,
  dashLayoutToDefaultPage,
  dashLayoutToPanelCounts,
}

// fetches user Dashboard pages from API
export function getDashPages(): DashPage[] {
  // TODO: get user pages from API
  return [
    dashLayoutToDefaultPage(DashLayout.ThreeOneTB),
    dashLayoutToDefaultPage(DashLayout.TwoTB),
    dashLayoutToDefaultPage(DashLayout.ThreeTwoLR),
  ];
}

// turns a dash layout enum into an empty DashPage with default dimensions
export function dashLayoutToDefaultPage(layout: DashLayout): DashPage {
  let elements: DashElement[] = [];
  switch(layout) {
    case DashLayout.One:
      elements.push({
        reportId: '1',
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
    case DashLayout.FourTB:
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
      reportId: '1',
      chartType: ChartType.BubbleCloud, // TODO: change this to none when reports are selectable
      width: col ? 50 : 100 / count,
      height: col ? 100 / count : 50
    });
  }
  return elements;
}

// returns an array where the first value represents the total number of panels,
// second and third are the counts of each row/column
export function dashLayoutToPanelCounts(layout: DashLayout): number[] {
  switch(layout) {
    case DashLayout.One:
      return [1, 1, 0];
    case DashLayout.TwoLR:
    case DashLayout.TwoTB:
      return [2, 1, 1];
    case DashLayout.ThreeOneLR:
    case DashLayout.ThreeOneTB:
      return [3, 1, 2];
    case DashLayout.ThreeTwoLR:
    case DashLayout.ThreeTwoTB:
      return [3, 2, 1];
    case DashLayout.FourTB:
      return [4, 2, 2];
    case DashLayout.FourOneLR:
    case DashLayout.FourOneTB:
      return [4, 1, 3];
    case DashLayout.FourThreeLR:
    case DashLayout.FourThreeTB:
      return [4, 3, 1];
    case DashLayout.FiveTwoLR:
    case DashLayout.FiveTwoTB:
      return [5, 2, 3];
    case DashLayout.FiveThreeLR:
    case DashLayout.FiveThreeTB:
      return [5, 3, 2];
  }
  return [0, 0, 0];
}
