using System;
using System.Threading.Tasks;
using Model;
using UnityEngine;
using UnityEngine.SceneManagement;
using UserInterface.Screens;

namespace Presenter
{
    public class Router : MonoBehaviour
    {
        [SerializeField] private GamePresenter _presenter;

        [SerializeField] private MenuScreen _menuScreen;

        [SerializeField] private GameScreen _gameScreen;

        [SerializeField] private EndScreen _endScreen;
        
        [SerializeField] private GameObject _gameAssets;

        [SerializeField] private DurationShuffleUpdater _durationShuffleUpdater;
        
        private PlayTable _playTable;

        private Score _score;

        private bool _playerChosen;

        private int _chosen;
        
        private const int StartGameDelayMilliseconds = 600;

        public async void StartGame()
        {
            Screen.autorotateToLandscapeLeft = true;

            Screen.autorotateToLandscapeRight = true;
            
            _gameAssets.SetActive(true);
            
            _gameScreen.gameObject.SetActive(true);
            
            _menuScreen.gameObject.SetActive(false);
            
            _durationShuffleUpdater.ResetDuration();

            await Task.Delay(StartGameDelayMilliseconds);
            
            await _presenter.MoveAllCupsUp();
            
            _playTable.MoveBallToRandomCup();
            
            await _presenter.MoveCoinToIndex(_playTable.GetCupIndexWithBall());

            await _presenter.MoveAllCupsNormal();

            bool win = true;

            while (win)
            {
                await _presenter.ShuffleCupsRandomCountTimes();

                _gameScreen.ChooseHelper.SetActive(true);
                
                await WaitPlayerChoose();

                _gameScreen.ChooseHelper.SetActive(false);
                
                await _presenter.MoveCupUp(_chosen);

                win = _chosen == _playTable.GetCupIndexWithBall();

                if (win)
                {
                    _score.Add();
                    
                    _durationShuffleUpdater.DeCreaseDuration();
                    
                    _playTable.MoveBallToRandomCup();
                }

                if (win)
                    await _presenter.MoveAllCupsUp();
                else
                    await _presenter.MoveCupUp(_playTable.GetCupIndexWithBall());

                if (!win) 
                    continue;
                
                _presenter.ResetCupsPositions();
                
                await _presenter.MoveCoinToIndex(_playTable.GetCupIndexWithBall());

                await _presenter.MoveAllCupsNormal();
            }
            
            _endScreen.gameObject.SetActive(true);
            
            _endScreen.UpdateCurrentScore(_score.Value);
            
            _endScreen.UpdateBestScore(_score.Best);
        }

        private void PlayerChosen(int index)
        {
            _playerChosen = true;

            _chosen = index;
        }
        
        private async Task WaitPlayerChoose()
        {
            _playerChosen = false;

            while (!_playerChosen) 
                await Task.Delay(1);
        }

        private void OpenMenu()
        {
            SceneManager.LoadScene("main");
        }

        private void Awake()
        {
            _score = new Score();
            
            _playTable = new PlayTable(3);
        }

        private void OnEnable()
        {
            _menuScreen.OnStartClicked += StartGame;
            _score.OnValueChanged += _gameScreen.UpdateScoreAsync;
            _durationShuffleUpdater.DurationUpdated += _presenter.UpdateDuration;
            _presenter.OnPressed += PlayerChosen;
            _endScreen.OnMenuClicked += OpenMenu;
        }

        private void Start()
        {
            _gameAssets.SetActive(false);
            
            _gameScreen.gameObject.SetActive(false);
            
            _menuScreen.gameObject.SetActive(true);
            
            _endScreen.gameObject.SetActive(false);

            _menuScreen.UpdateBestScore(_score.Best);
            
            _gameScreen.UpdateScoreAsync(0);
            
            _gameScreen.ChooseHelper.SetActive(false);

            Application.targetFrameRate = 60;

            Screen.orientation = ScreenOrientation.Portrait;
        }

        private void OnDisable()
        {
            _menuScreen.OnStartClicked -= StartGame;
            _score.OnValueChanged -= _gameScreen.UpdateScoreAsync;
            _durationShuffleUpdater.DurationUpdated -= _presenter.UpdateDuration;
            _presenter.OnPressed -= PlayerChosen;
            _endScreen.OnMenuClicked -= OpenMenu;
        }
    }
}