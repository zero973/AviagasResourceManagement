export default class BaseListParams {

  constructor(PageNumber, RowsCount, OrderBy, Filters)
  {
    this.PageNumber = PageNumber;
    this.RowsCount = RowsCount;
    this.OrderBy = OrderBy;
    this.Filters = Filters;
  }

}
