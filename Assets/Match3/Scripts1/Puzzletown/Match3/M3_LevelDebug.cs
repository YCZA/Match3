using DG.Tweening;

namespace Match3.Scripts1.Puzzletown.Match3
{
    public class M3_LevelDebug
    {
        private LevelLoader levelLoader;
		private DoobersRoot doobers;

		public M3_LevelDebug(LevelLoader levelLoader, DoobersRoot doobers)
		{
			this.levelLoader = levelLoader;
			this.doobers = doobers;
		}
		
		public void FinishGame(bool success, int movesTaken = 0)
		{
			this.StopAllRunningAnimations();
			this.levelLoader.ScoringController.FinishLevel(success, movesTaken);
		}
		
		private void StopAllRunningAnimations()
		{
			this.levelLoader.BoardView.BoardAnimationController.StopAllCoroutines();
			this.levelLoader.BoardView.StopAllCoroutines();
			this.doobers.StopAllCoroutines();
			DOTween.Clear(false);
		}
    }
}