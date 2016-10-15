namespace TfsWorkspaceUpdater.Core.Views
{
    using Shared.Views;

    public abstract class BasePresenter<TView, TViewModel> : IPresenter
        where TView : IView<TViewModel>
        where TViewModel : IViewModel
    {
        ////////////////////////////////////////////////////////////////////////////////////////////////////////
        #region Properties

        protected TView View { get; }

        protected TViewModel ViewModel { get; }

        #endregion

        ////////////////////////////////////////////////////////////////////////////////////////////////////////
        #region Constructors

        protected BasePresenter(TView view, TViewModel viewModel)
        {
            View = view;
            ViewModel = viewModel;

            View.ConnectDataSource(ViewModel);
        }

        #endregion
    }

    public abstract class BasePresenter<TView, TViewModel, TModel> : BasePresenter<TView, TViewModel>
        where TView : IView<TViewModel>
        where TViewModel : IViewModel
        where TModel : IModel
    {
        ////////////////////////////////////////////////////////////////////////////////////////////////////////
        #region Properties

        protected TModel Model { get; }

        #endregion

        ////////////////////////////////////////////////////////////////////////////////////////////////////////
        #region Constructors

        protected BasePresenter(TView view, TViewModel viewModel, TModel model)
            : base(view, viewModel)
        {
            Model = model;
        }

        #endregion
    }
}